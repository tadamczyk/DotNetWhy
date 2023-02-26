namespace DotNetWhy.Services;

public static class Service
{
    private static readonly IServiceCollection ServiceCollection = new ServiceCollection();

    public static void Run(IReadOnlyCollection<string> arguments)
    {
        var parameters = new Parameters(arguments);

        ServiceCollection
            .AddSingleton<IParameters>(parameters)
            .AddServices()
            .BuildServiceProvider()
            .GetRequiredService<IDotNetWhyService>()
            ?.Run(parameters);
    }
}