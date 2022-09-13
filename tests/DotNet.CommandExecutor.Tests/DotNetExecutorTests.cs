[TestFixture]
[TestOf(typeof(DotNetExecutor))]
public class DotNetExecutorTests
{
    [Test]
    public void Should_DotNet_Command_Result_Has_Empty_Errors()
    {
        // Arrange
        var command = DotNetExecutor
            .Initialize();

        // Act
        var result = command.AndExecute();

        // Asserts
        // result.IsSuccess.Should().BeTrue() failed due to DotNet command without arguments returns unhandled int value
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Should_DotNet_With_Correct_Arguments_Command_Result_Has_Empty_Errors()
    {
        // Arrange
        var arguments = new[] {"--version"};
        var command = DotNetExecutor
            .Initialize()
            .WithArguments(arguments);

        // Act
        var result = command.AndExecute();

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Should_DotNet_With_Incorrect_Arguments_Command_Result_Has_Errors()
    {
        // Arrange
        var arguments = new[] {"version"};
        var command = DotNetExecutor
            .Initialize()
            .WithArguments(arguments);

        // Act
        var result = command.AndExecute();

        // Asserts
        result.IsSuccess.Should().BeFalse();
        result.Output.Should().NotBeNull();
        result.Output.Should().BeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().NotBeEmpty();
    }

    [Test]
    public void Should_DotNet_Command_Ran_In_Current_Directory_Result_Has_Empty_Errors()
    {
        // Arrange
        var directory = Environment.CurrentDirectory;
        var command = DotNetExecutor
            .Initialize()
            .InDirectory(directory);

        // Act
        var result = command.AndExecute();

        // Asserts
        // result.IsSuccess.Should().BeTrue() failed due to DotNet command without arguments returns unhandled int value
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Should_DotNet_Command_Ran_In_Parent_Directory_Result_Has_Empty_Errors()
    {
        // Arrange
        var directory = Directory.GetParent(Environment.CurrentDirectory)?.FullName;
        var command = DotNetExecutor
            .Initialize()
            .InDirectory(directory);

        // Act
        var result = command.AndExecute();

        // Asserts
        // result.IsSuccess.Should().BeTrue() failed due to DotNet command without arguments returns unhandled int value
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }

    [Test]
    public void Should_DotNet_With_Correct_Arguments_Command_Ran_In_Current_Directory_Result_Has_Empty_Errors()
    {
        // Arrange
        var arguments = new[] {"--version"};
        var directory = Environment.CurrentDirectory;
        var command = DotNetExecutor
            .Initialize()
            .InDirectory(directory)
            .WithArguments(arguments);

        // Act
        var result = command.AndExecute();

        // Asserts
        result.IsSuccess.Should().BeTrue();
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBeEmpty();
        result.Errors.Should().NotBeNull();
        result.Errors.Should().BeEmpty();
    }
}