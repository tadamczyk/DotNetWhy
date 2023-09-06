namespace DotNetWhy.Domain.Tests;

[TestFixture(TestOf = typeof(DependencyTreeProvider))]
public class DependencyTreeProviderTests
{
    private IMediator _mediator;

    private DependencyTreeProvider _sut;

    [SetUp]
    public void SetUp()
    {
        _mediator = Substitute.For<IMediator>();

        _sut = new DependencyTreeProvider(_mediator);
    }

    [Test]
    public async Task Should_Return_Tree_With_Only_Root()
    {
        // Arrange
        var parameters = new DependencyTreeParameters(
            "WorkingDirectory",
            "PackageName",
            "PackageVersion");

        var expectedRoot = new DependencyTreeNode(
            "Name",
            "Version");

        _mediator
            .SendAsync(Arg.Any<RestoreProjectCommand>())
            .Returns(Task.CompletedTask);

        _mediator
            .SendAsync(Arg.Any<GenerateRestoreGraphFileCommand>())
            .Returns(Task.CompletedTask);

        _mediator
            .SendAsync<GetDependencyTreeQuery, DependencyTreeNode>(Arg.Any<GetDependencyTreeQuery>())
            .Returns(Task.FromResult(expectedRoot));

        // Act
        var result = await _sut.GetAsync(parameters);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(expectedRoot.Name);
        result.Version.Should().Be(expectedRoot.Version);
        result.LastNodesSum.Should().Be(1);
        result.HasNodes.Should().BeFalse();
        result.Nodes.Should().BeEmpty();
    }

    [Test]
    public async Task Should_Throw_RestoreProjectFailedException_When_RestoreProjectCommandHandler_Failed()
    {
        // Arrange
        var workingDirectory = "WorkingDirectory";

        var parameters = new DependencyTreeParameters(
            workingDirectory,
            "PackageName",
            "PackageVersion");

        _mediator
            .SendAsync(Arg.Any<RestoreProjectCommand>())
            .Returns(Task.FromException(new RestoreProjectFailedException(workingDirectory)));

        _mediator
            .SendAsync(Arg.Any<GenerateRestoreGraphFileCommand>())
            .Returns(Task.CompletedTask);

        // Act
        Func<Task> result = () => _sut.GetAsync(parameters);

        // Assert
        await result.Should().ThrowAsync<RestoreProjectFailedException>();
    }

    [Test]
    public async Task Should_Throw_GenerateRestoreGraphFileFailedException_When_GenerateRestoreGraphFileCommandHandler_Failed()
    {
        // Arrange
        var workingDirectory = "WorkingDirectory";

        var parameters = new DependencyTreeParameters(
            workingDirectory,
            "PackageName",
            "PackageVersion");

        _mediator
            .SendAsync(Arg.Any<RestoreProjectCommand>())
            .Returns(Task.CompletedTask);

        _mediator
            .SendAsync(Arg.Any<GenerateRestoreGraphFileCommand>())
            .Returns(Task.FromException(new GenerateRestoreGraphFileFailedException(workingDirectory)));

        // Act
        Func<Task> result = () => _sut.GetAsync(parameters);

        // Assert
        await result.Should().ThrowAsync<GenerateRestoreGraphFileFailedException>();
    }

    [Test]
    public async Task Should_Throw_Exception_When_GetDependencyTreeQueryHandler_Failed()
    {
        // Arrange
        var parameters = new DependencyTreeParameters(
            "WorkingDirectory",
            "PackageName",
            "PackageVersion");

        var expectedException = new Exception("Failed");

        _mediator
            .SendAsync(Arg.Any<RestoreProjectCommand>())
            .Returns(Task.CompletedTask);

        _mediator
            .SendAsync(Arg.Any<GenerateRestoreGraphFileCommand>())
            .Returns(Task.CompletedTask);

        _mediator
            .SendAsync<GetDependencyTreeQuery, DependencyTreeNode>(Arg.Any<GetDependencyTreeQuery>())
            .Returns(Task.FromException<DependencyTreeNode>(expectedException));

        // Act
        Func<Task> result = () => _sut.GetAsync(parameters);

        // Assert
        await result.Should().ThrowAsync<Exception>(expectedException.Message);
    }
}