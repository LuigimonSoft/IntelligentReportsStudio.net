using Xunit;

namespace IntelligentReportStudio.UnitTests;

public class LibraryInfoTests
{
    [Fact]
    public void Name_IdentifiesTheLibrary()
    {
        Assert.Equal("IntelligentReportStudio", LibraryInfo.Name);
    }
}
