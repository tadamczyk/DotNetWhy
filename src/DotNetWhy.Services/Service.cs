namespace DotNetWhy.Services;

public static class Service
{
    private static readonly IServiceCollection ServiceCollection = new ServiceCollection();

    public static async Task RunAsync(IReadOnlyCollection<string> arguments)
    {
        var parameters = new Parameters(arguments);

        await ServiceCollection
            .AddSingleton<IParameters>(parameters)
            .AddServices()
            .BuildServiceProvider()
            .GetRequiredService<IDotNetWhyService>()
            ?.RunAsync(parameters)!;
    }
}