namespace DotNetWhy.Services.Interfaces;

internal interface IDependencyGraphLogger
{
    void Log(Solution solution, string packageName);
}