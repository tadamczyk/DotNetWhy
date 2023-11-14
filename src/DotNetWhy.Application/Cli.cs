namespace DotNetWhy.Application;

public static class Cli
{
    public static void Run(IEnumerable<string> args) =>
        ApplicationBuilder
            .AddServices()
            .AsTypeRegistrar()
            .ForCommandApplication<DotNetWhyCommand>("dotnet why")
            .Run(args);
}