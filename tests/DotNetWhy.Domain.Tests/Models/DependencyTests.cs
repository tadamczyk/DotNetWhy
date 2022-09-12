[TestFixture]
[TestOf(typeof(Dependency))]
public class DependencyTests
{
    [Test]
    public void Should_Initialize_Dependency_Correctly()
    {
        // Arrange
        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";

        // Act
        var result = new Dependency(dependencyName1, dependencyVersion1);

        // Asserts
        result.Should().NotBeNull();
        result.Name.Should().Be(dependencyName1);
        result.Version.Should().Be(dependencyVersion1);
        result.Dependencies.Count.Should().Be(0);
        result.DependencyPathCounter.Should().Be(1);
        result.HasDependencies.Should().BeFalse();
        result.ToString().Should().Be($"{dependencyName1} ({dependencyVersion1})");
    }

    [Test]
    public void Should_Add_Dependency_Correctly()
    {
        // Arrange
        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";
        var dependency1 = new Dependency(dependencyName1, dependencyVersion1);

        var dependencyName2 = "Name2";
        var dependencyVersion2 = "Version2";
        var dependency2 = new Dependency(dependencyName2, dependencyVersion2);

        // Act
        dependency1.AddDependency(dependency2);

        // Asserts
        dependency1.Dependencies.Count.Should().Be(1);
        dependency1.DependencyPathCounter.Should().Be(1);
        dependency1.HasDependencies.Should().BeTrue();
    }

    [Test]
    public void Should_Add_Dependencies_Correctly()
    {
        // Arrange
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

        var dependencyName5 = "Name5";
        var dependencyVersion5 = "Version5";
        var dependency5 = new Dependency(dependencyName5, dependencyVersion5);

        // Act
        dependency1.AddDependency(dependency2);
        dependency2.AddDependency(dependency3);
        dependency3.AddDependency(dependency5);
        dependency1.AddDependency(dependency4);

        // Asserts
        dependency1.Dependencies.Count.Should().Be(2);
        dependency1.DependencyPathCounter.Should().Be(2);
        dependency1.HasDependencies.Should().BeTrue();

        dependency2.Dependencies.Count.Should().Be(1);
        dependency2.DependencyPathCounter.Should().Be(1);
        dependency2.HasDependencies.Should().BeTrue();

        dependency3.Dependencies.Count.Should().Be(1);
        dependency3.DependencyPathCounter.Should().Be(1);
        dependency3.HasDependencies.Should().BeTrue();

        dependency4.Dependencies.Count.Should().Be(0);
        dependency4.DependencyPathCounter.Should().Be(1);
        dependency4.HasDependencies.Should().BeFalse();

        dependency5.Dependencies.Count.Should().Be(0);
        dependency5.DependencyPathCounter.Should().Be(1);
        dependency5.HasDependencies.Should().BeFalse();
    }

    [Test]
    public void Should_Check_If_Is_Or_Contains_Package_Returns_True()
    {
        // Arrange
        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";
        var dependency1 = new Dependency(dependencyName1, dependencyVersion1);

        var dependencyName2 = "Name2";
        var dependencyVersion2 = "Version2";
        var dependency2 = new Dependency(dependencyName2, dependencyVersion2);

        dependency1.AddDependency(dependency2);

        // Act
        var result = dependency1.IsOrContainsPackage(dependencyName2);

        // Asserts
        result.Should().BeTrue();
    }

    [Test]
    public void Should_Check_If_Is_Or_Contains_Package_Returns_False()
    {
        // Arrange
        var dependencyName1 = "Name1";
        var dependencyVersion1 = "Version1";
        var dependency1 = new Dependency(dependencyName1, dependencyVersion1);

        var dependencyName2 = "Name2";
        var dependencyVersion2 = "Version2";
        var dependency2 = new Dependency(dependencyName2, dependencyVersion2);

        dependency1.AddDependency(dependency2);

        // Act
        var result1 = dependency1.IsOrContainsPackage("Name3");
        var result2 = dependency2.IsOrContainsPackage("Name3");

        // Asserts
        result1.Should().BeTrue();
        result2.Should().BeFalse();
    }
}