namespace DotNet.Cli.Tests;

[TestFixture(TestOf = typeof(Command))]
public class CommandTests
{
    private Command _command;

    [SetUp]
    public void SetUp() =>
        _command = new Command();

    [Test]
    public void Should_Execute_Command_With_No_Error_As_Result()
    {
        // Act
        var result = _command.Execute();

        // Assert
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBe(string.Empty);
        result.Error.Should().NotBeNull();
        result.Error.Should().Be(string.Empty);
    }

    [TestCase("-h")]
    [TestCase("--help")]
    [TestCase("--info")]
    [TestCase("--version")]
    public void Should_Execute_Command_With_Arguments_And_Success_With_No_Error_As_Result(string arguments)
    {
        // Act
        var result =
            _command
                .WithArguments(arguments)
                .Execute();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBe(string.Empty);
        result.Error.Should().NotBeNull();
        result.Error.Should().Be(string.Empty);
    }

    [Test]
    public void Should_Execute_Command_With_Array_Arguments_And_Success_With_No_Error_As_Result()
    {
        // Arrange
        var arguments = new[] {"tool", "list"};

        // Act
        var result =
            _command
                .WithArguments(arguments)
                .Execute();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBe(string.Empty);
        result.Error.Should().NotBeNull();
        result.Error.Should().Be(string.Empty);
    }

    [Test]
    public void Should_Execute_Command_With_Arguments_And_With_Working_Directory_And_Success_With_No_Error_As_Result()
    {
        // Arrange
        var arguments = "list package";
        var workingDirectory =
            Directory.GetParent(Environment.CurrentDirectory)!.Parent!.Parent!.Parent!.Parent!.FullName;

        // Act
        var result =
            _command
                .WithArguments(arguments)
                .WithWorkingDirectory(workingDirectory)
                .Execute();

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Output.Should().NotBeNull();
        result.Output.Should().NotBe(string.Empty);
        result.Error.Should().NotBeNull();
        result.Error.Should().Be(string.Empty);
    }

    [Test]
    public void Should_Execute_Command_With_Incorrect_Arguments_And_Failed_With_Error_As_Result()
    {
        // Arrange
        var arguments = "version";

        // Act
        var result =
            _command
                .WithArguments(arguments)
                .Execute();

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Output.Should().NotBeNull();
        result.Output.Should().Be(string.Empty);
        result.Error.Should().NotBeNull();
        result.Error.Should().NotBe(string.Empty);
    }
}