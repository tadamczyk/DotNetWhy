namespace DotNetWhy.Services;

public interface IDotNetWhyService
{
    Task RunAsync(IParameters parameters);
}