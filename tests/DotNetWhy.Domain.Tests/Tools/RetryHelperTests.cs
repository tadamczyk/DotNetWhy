[TestFixture]
[TestOf(typeof(RetryHelper))]
public class RetryHelperTests
{
    [Test]
    public void Should_Allow_Retry_Again()
    {
        // Arrange
        var retry = new RetryHelper();

        // Act
        var result = retry.CanTryAgain();

        // Asserts
        result.Should().BeTrue();
    }

    [Test]
    public void Should_Allow_Retry_Again_Only_Three_Times()
    {
        // Arrange
        var retry = new RetryHelper();
        var repeats = 5;
        var results = new bool[5];

        // Act
        for (var iterator = 0; iterator < repeats; iterator++) results[iterator] = retry.CanTryAgain();

        // Asserts
        results[0].Should().BeTrue();
        results[1].Should().BeTrue();
        results[2].Should().BeTrue();
        results[3].Should().BeFalse();
        results[4].Should().BeFalse();
    }

    [Test]
    public void Should_Allow_Retry_Again_All_Times_When_Max_Retry_Is_Equal_Repeats()
    {
        // Arrange
        var repeats = 5;
        var retry = new RetryHelper(repeats);
        var results = new bool[5];

        // Act
        for (var iterator = 0; iterator < repeats; iterator++) results[iterator] = retry.CanTryAgain();

        // Asserts
        results.All(result => result).Should().BeTrue();
    }
}