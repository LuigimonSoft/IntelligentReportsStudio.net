using Xunit;

namespace IntelligentReportStudio.CompatibilityTests;

public class SmokeTests
{
    [Fact]
    public void LibraryAssemblyIsAvailable()
    {
        Assert.Equal("IntelligentReportStudio", typeof(LibraryMarker).Assembly.GetName().Name);
    }
}
