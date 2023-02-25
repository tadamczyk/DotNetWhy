[TestFixture]
[TestOf(typeof(Project))]
public class ProjectTests
{
    [Test]
    public void Should_Initialize_Project_Correctly()
    {
        // Arrange
        var projectName1 = "Name1";

        // Act
        var result = new Project(projectName1);

        // Asserts
        result.Should().NotBeNull();
        result.Name.Should().Be(projectName1);
        result.Targets.Count.Should().Be(0);
        result.DependencyPathCounter.Should().Be(0);
        result.HasTargets.Should().BeFalse();
    }

    [Test]
    public void Should_Add_Target_Correctly()
    {
        // Arrange
        var projectName1 = "Name1";
        var project1 = new Project(projectName1);

        var targetName1 = "Name1";
        var target1 = new Target(targetName1);

        // Act
        project1.AddTarget(target1);

        // Asserts
        project1.Targets.Count.Should().Be(1);
        project1.DependencyPathCounter.Should().Be(0);
        project1.HasTargets.Should().BeTrue();
    }

    [Test]
    public void Should_Add_Targets_With_Dependencies_Correctly()
    {
        // Arrange
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
        project1.AddTarget(target1);
        target1.AddDependency(dependency1);
        dependency1.AddDependency(dependency2);
        dependency2.AddDependency(dependency4);
        target1.AddDependency(dependency3);

        // Asserts
        project1.Targets.Count.Should().Be(1);
        project1.DependencyPathCounter.Should().Be(2);
        project1.HasTargets.Should().BeTrue();
    }
}