[TestFixture]
[TestOf(typeof(Target))]
public class TargetTests
{
    [Test]
    public void Should_Initialize_Target_Correctly()
    {
        // Arrange
        var targetName1 = "Name1";

        // Act
        var result = new Target(targetName1);

        // Asserts
        result.Should().NotBeNull();
        result.Name.Should().Be(targetName1);
        result.Dependencies.Count.Should().Be(0);
        result.DependencyPathCounter.Should().Be(0);
        result.HasDependencies.Should().BeFalse();
    }

    [Test]
    public void Should_Add_Dependency_Correctly()
    {
        // Arrange
        var targetName1 = "Name1";
        var target1 = new Target(targetName1);

        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";
        var dependency1 = new Dependency(dependencyName1, dependencyVersion1);

        // Act
        target1.AddDependency(dependency1);

        // Asserts
        target1.Dependencies.Count.Should().Be(1);
        target1.DependencyPathCounter.Should().Be(1);
        target1.HasDependencies.Should().BeTrue();
    }

    [Test]
    public void Should_Add_Dependencies_Correctly()
    {
        // Arrange
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
        target1.AddDependency(dependency1);
        dependency1.AddDependency(dependency2);
        dependency2.AddDependency(dependency4);
        target1.AddDependency(dependency3);

        // Asserts
        target1.Dependencies.Count.Should().Be(2);
        target1.DependencyPathCounter.Should().Be(2);
        target1.HasDependencies.Should().BeTrue();
    }
}