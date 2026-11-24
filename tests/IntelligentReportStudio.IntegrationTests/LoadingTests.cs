using System.Text;
using IntelligentReportStudio;
using Xunit;

namespace IntelligentReportStudio.IntegrationTests;

public sealed class LoadingTests
{
    [Fact]
    public async Task StreamLoadingLeavesTheSourceOpen()
    {
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes("{\"version\":1,\"report\":{\"version\":1}}"));
        var definition = await ReportDefinition.LoadAsync(stream);
        Assert.True(stream.CanRead);
        Assert.NotNull(definition.ToModel().Properties["report"]);
    }
}
