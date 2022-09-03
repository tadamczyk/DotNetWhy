namespace DotNetWhy.Services;

public interface IDotNetWhyService
{
    void Run(IReadOnlyCollection<string> arguments);
}