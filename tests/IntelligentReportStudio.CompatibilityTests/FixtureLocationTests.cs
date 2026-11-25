using Xunit;

namespace IntelligentReportStudio.CompatibilityTests;

public sealed class FixtureLocationTests
{
    [Fact]
    public void ReferenceFixtureLocationIsWithinTestOutput()
    {
        var location = Path.Combine(AppContext.BaseDirectory, "fixtures", "reference");
        Assert.StartsWith(AppContext.BaseDirectory, location, StringComparison.Ordinal);
    }
}
