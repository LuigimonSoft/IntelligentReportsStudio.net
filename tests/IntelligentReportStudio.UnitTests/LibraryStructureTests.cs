using Xunit;

namespace IntelligentReportStudio.UnitTests;

public sealed class LibraryStructureTests
{
    [Fact]
    public void RequiredLibraryAreasContainSourceFiles()
    {
        var root = FindRoot();
        var library = Path.Combine(root, "src", "IntelligentReportStudio");
        Assert.True(File.Exists(Path.Combine(library, "IntelligentReportStudio.csproj")));

        foreach (var area in new[] { "Application", "Services", "Repository", "Models/Input", "Models/Output" })
        {
            var directory = Path.Combine(library, area.Replace('/', Path.DirectorySeparatorChar));
            Assert.True(Directory.Exists(directory), $"Missing library area: {area}");
            var sources = Directory.EnumerateFiles(directory, "*.cs", SearchOption.AllDirectories)
                .Where(path => !Path.GetRelativePath(library, path)
                    .Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    .Any(part => part is "bin" or "obj"));
            Assert.True(sources.Any(), $"No source files found in library area: {area}");
        }
    }

    private static string FindRoot()
    {
        for (var directory = new DirectoryInfo(AppContext.BaseDirectory); directory is not null; directory = directory.Parent)
            if (File.Exists(Path.Combine(directory.FullName, "IntelligentReportStudio.slnx")))
                return directory.FullName;
        throw new DirectoryNotFoundException("Could not locate IntelligentReportStudio.slnx.");
    }
}
