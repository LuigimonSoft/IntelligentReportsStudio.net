using Xunit;

namespace IntelligentReportStudio.BehaviorTests;

public class LibraryIdentityTests
{
    [Fact]
    public void GivenTheLibrary_WhenItsNameIsRequested_ThenTheNameIsAvailable()
    {
        Assert.False(string.IsNullOrWhiteSpace(LibraryInfo.Name));
    }
}
