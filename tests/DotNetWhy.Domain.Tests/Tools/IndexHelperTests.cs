[TestFixture]
[TestOf(typeof(IndexHelper))]
public class IndexHelperTests
{
    [Test]
    public void Should_Get_Next_Value_Only_Once()
    {
        // Arrange
        var index = new IndexHelper();

        // Act
        index.Next();

        // Asserts
        index.Should().NotBeNull();
        index.Value.Should().Be(1);
    }

    [Test]
    public void Should_Get_Next_Value_Multiple_Times()
    {
        // Arrange
        var index = new IndexHelper();
        var repeats = 10;

        // Act
        for (var iterator = 0; iterator < repeats; iterator++) index.Next();

        // Asserts
        index.Should().NotBeNull();
        index.Value.Should().Be(repeats);
    }

    [Test]
    public void Should_Reset_Index_Value_Correctly()
    {
        // Arrange
        var index = new IndexHelper();
        var repeats = 10;

        for (var iterator = 0; iterator < repeats; iterator++) index.Next();

        // Act
        index.Reset();

        // Asserts
        index.Should().NotBeNull();
        index.Value.Should().Be(0);
    }
}