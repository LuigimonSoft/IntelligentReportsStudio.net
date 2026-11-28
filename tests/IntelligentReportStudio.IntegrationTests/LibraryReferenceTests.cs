using Xunit;

namespace IntelligentReportStudio.IntegrationTests;

public class LibraryReferenceTests
{
    [Fact]
    public void LibraryAssembly_IsAvailableToTheTestProject()
    {
        Assert.Equal("IntelligentReportStudio", typeof(LibraryInfo).Assembly.GetName().Name);
    }
}
