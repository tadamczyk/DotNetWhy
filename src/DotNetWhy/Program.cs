namespace DotNetWhy;

internal static class Program
{
    private static readonly IDotNetWhyService Service;

    static Program() =>
        Service = IServiceCollectionExtensions.GetDotNetWhyService();

    internal static void Main(string[] args) =>
        Service.Run(args);
}