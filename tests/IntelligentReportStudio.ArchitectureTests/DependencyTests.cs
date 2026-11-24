using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Xunit;

namespace IntelligentReportStudio.ArchitectureTests;

public sealed class DependencyTests
{
    [Fact]
    public void LayersDoNotReferenceForbiddenDependencies()
    {
        var sourceRoot = Path.Combine(FindRoot(), "src", "IntelligentReportStudio");
        var files = Directory.GetFiles(sourceRoot, "*.cs", SearchOption.AllDirectories)
            .Where(path => !Parts(Path.GetRelativePath(sourceRoot, path))
                .Any(part => part is "bin" or "obj"))
            .OrderBy(path => path, StringComparer.Ordinal)
            .ToArray();
        Assert.NotEmpty(files);

        var trees = files.Select(path => CSharpSyntaxTree.ParseText(File.ReadAllText(path), path: path)).ToArray();
        var frameworkPaths = (string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES");
        Assert.False(string.IsNullOrEmpty(frameworkPaths), "Framework references are unavailable.");
        var references = frameworkPaths!.Split(Path.PathSeparator)
            .Select(path => MetadataReference.CreateFromFile(path));
        var globals = CSharpSyntaxTree.ParseText(
            "global using System; global using System.Collections.Generic; global using System.IO; " +
            "global using System.Linq; global using System.Net.Http; global using System.Threading; " +
            "global using System.Threading.Tasks; global using IntelligentReportStudio.Services;");
        var compilation = CSharpCompilation.Create("ArchitectureAnalysis", trees.Append(globals), references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var assembly = new MemoryStream();
        var emit = compilation.Emit(assembly);
        Assert.True(emit.Success, "Architecture analysis could not compile the library:\n" +
            string.Join("\n", emit.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error)));

        var violations = new SortedSet<string>(StringComparer.Ordinal);
        foreach (var tree in trees)
        {
            var sourcePath = Path.GetRelativePath(sourceRoot, tree.FilePath);
            var sourceLayer = Layer(sourcePath);
            var semanticModel = compilation.GetSemanticModel(tree);

            foreach (var declaration in tree.GetRoot().DescendantNodes().OfType<BaseNamespaceDeclarationSyntax>())
            {
                if (semanticModel.GetDeclaredSymbol(declaration) is INamespaceSymbol ns && IsGraphical(ns.ToDisplayString()))
                    violations.Add($"{sourcePath} -> graphical framework namespace {ns.ToDisplayString()}");
            }

            foreach (var node in tree.GetRoot().DescendantNodes().OfType<SimpleNameSyntax>())
            {
                var symbol = semanticModel.GetSymbolInfo(node).Symbol;
                if (symbol is IAliasSymbol alias) symbol = alias.Target;
                var target = symbol as INamedTypeSymbol ?? symbol?.ContainingType;
                if (target is null) continue;
                target = target.OriginalDefinition;
                var line = node.GetLocation().GetLineSpan().StartLinePosition.Line + 1;
                var targetName = target.ToDisplayString();
                var targetNamespace = target.ContainingNamespace?.ToDisplayString() ?? string.Empty;
                if (IsGraphical(targetNamespace) || IsGraphicalAssembly(target.ContainingAssembly?.Name))
                    violations.Add($"{sourcePath}:{line} -> graphical framework type {targetName}");

                foreach (var location in target.Locations.Where(location => location.IsInSource && location.SourceTree is not null))
                {
                    var targetPath = location.SourceTree!.FilePath;
                    if (string.IsNullOrEmpty(targetPath)) continue;
                    var targetLayer = Layer(Path.GetRelativePath(sourceRoot, targetPath));
                    if (Forbidden(sourceLayer, targetLayer))
                        violations.Add($"{sourcePath}:{line} ({sourceLayer}) -> {targetName} ({targetLayer})");
                }
            }
        }

        foreach (var reference in compilation.ReferencedAssemblyNames)
        {
            if (IsGraphicalAssembly(reference.Name))
                violations.Add($"Library compilation -> graphical framework assembly {reference.Name}");
        }

        Assert.True(violations.Count == 0, "Forbidden architecture dependencies:\n" + string.Join("\n", violations));
    }

    private static bool Forbidden(string source, string target)
    {
        var sourceIsModel = source.StartsWith("Models", StringComparison.Ordinal);
        var targetIsModel = target.StartsWith("Models", StringComparison.Ordinal);
        return (sourceIsModel && target is "Application" or "Services" or "Repository")
            || (source == "Models/Input" && target == "Models/Output")
            || (source == "Models/Output" && target == "Models/Input")
            || (source == "Services" && target is "Application" or "Repository")
            || (source == "Repository" && target == "Application");
    }

    private static bool IsGraphical(string name) => new[]
    {
        "System.Windows.Forms", "System.Windows.Controls", "System.Windows.Media",
        "Microsoft.Maui", "Microsoft.AspNetCore", "Microsoft.UI", "Windows.UI",
        "Avalonia", "System.Web.UI"
    }.Any(prefix => name == prefix || name.StartsWith(prefix + ".", StringComparison.Ordinal));

    private static bool IsGraphicalAssembly(string? name) => name is not null && new[]
    {
        "System.Windows.Forms", "PresentationFramework", "PresentationCore", "WindowsBase",
        "Microsoft.Maui", "Microsoft.AspNetCore", "Microsoft.WindowsDesktop", "Microsoft.UI",
        "Avalonia"
    }.Any(prefix => name.Equals(prefix, StringComparison.OrdinalIgnoreCase) ||
        name.StartsWith(prefix + ".", StringComparison.OrdinalIgnoreCase));

    private static string Layer(string path)
    {
        var parts = Parts(path);
        if (parts.Length == 0) return string.Empty;
        return parts[0] == "Models" && parts.Length > 1 && parts[1] is "Input" or "Output"
            ? $"Models/{parts[1]}" : parts[0];
    }

    private static string[] Parts(string path) =>
        path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

    private static string FindRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "IntelligentReportStudio.slnx"))) return directory.FullName;
        throw new DirectoryNotFoundException("Could not locate IntelligentReportStudio.slnx for architecture analysis.");
    }
}
