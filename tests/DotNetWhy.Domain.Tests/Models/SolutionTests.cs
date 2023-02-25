[TestFixture]
[TestOf(typeof(Solution))]
public class SolutionTests
{
    [Test]
    public void Should_Initialize_Solution_Correctly()
    {
        // Arrange
        var solutionName1 = "Name1";

        // Act
        var result = new Solution(solutionName1);

        // Asserts
        result.Should().NotBeNull();
        result.Name.Should().Be(solutionName1);
        result.Projects.Count.Should().Be(0);
        result.DependencyPathCounter.Should().Be(0);
        result.HasProjects.Should().BeFalse();
    }

    [Test]
    public void Should_Add_Project_Correctly()
    {
        // Arrange
        var solutionName1 = "Name1";
        var solution1 = new Solution(solutionName1);

        var projectName1 = "Name1";
        var project1 = new Project(projectName1);

        // Act
        solution1.AddProject(project1);

        // Asserts
        solution1.Projects.Count.Should().Be(1);
        solution1.DependencyPathCounter.Should().Be(0);
        solution1.HasProjects.Should().BeTrue();
    }

    [Test]
    public void Should_Add_Projects_With_Targets_With_Dependencies_Correctly()
    {
        // Arrange
        var solutionName1 = "Name1";
        var solution1 = new Solution(solutionName1);

        var projectName1 = "Name1";
        var project1 = new Project(projectName1);

        var targetName1 = "Name1";
        var target1 = new Target(targetName1);

        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";
        var dependency1 = new Dependency(dependencyName1, dependencyVersion1);

        var dependencyName2 = "Name2";
        var dependencyVersion2 = "Version2";
        var dependency2 = new Dependency(dependencyName2, dependencyVersion2);

        var dependencyName3 = "Name3";
        var dependencyVersion3 = "Version3";
        var dependency3 = new Dependency(dependencyName3, dependencyVersion3);

        var dependencyName4 = "Name4";
        var dependencyVersion4 = "Version4";
        var dependency4 = new Dependency(dependencyName4, dependencyVersion4);

        // Act
        solution1.AddProject(project1);
        project1.AddTarget(target1);
        target1.AddDependency(dependency1);
        dependency1.AddDependency(dependency2);
        dependency2.AddDependency(dependency4);
        target1.AddDependency(dependency3);

        // Asserts
        solution1.Projects.Count.Should().Be(1);
        solution1.DependencyPathCounter.Should().Be(2);
        solution1.HasProjects.Should().BeTrue();
    }
}