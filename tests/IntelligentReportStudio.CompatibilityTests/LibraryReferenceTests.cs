using Xunit;

namespace IntelligentReportStudio.CompatibilityTests;

public class LibraryReferenceTests
{
    [Fact]
    public void LibraryName_IsAccessibleFromCompatibilityTests()
    {
        Assert.Equal("IntelligentReportStudio", LibraryInfo.Name);
    }
}
