using Xunit;

namespace IntelligentReportStudio.BehaviorTests;

public class SmokeTests
{
    [Fact]
    public void LibraryAssemblyIsAvailable()
    {
        Assert.Equal("IntelligentReportStudio", typeof(LibraryMarker).Assembly.GetName().Name);
    }
}
