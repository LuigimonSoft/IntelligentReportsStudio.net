using IntelligentReportStudio;
using Xunit;

namespace IntelligentReportStudio.BehaviorTests;

public sealed class DefinitionLoadingScenario
{
    [Fact]
    public void TemplateWithoutSchemaVersionCanBeLoadedAndCopied()
    {
        // Given a report with root and report versions
        const string json = "{\"version\":1,\"report\":{\"version\":1}}";

        // When the definition is loaded
        var definition = ReportDefinition.FromJson(json);

        // Then the definition can be copied independently
        var copy = definition.ToModel();
        copy.Properties["version"] = 2;
        Assert.Equal(1, definition.ToModel().Properties["version"]!.GetValue<int>());
    }
}
