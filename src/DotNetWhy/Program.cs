namespace DotNetWhy;

internal static class Program
{
    internal static async Task Main(string[] args) =>
        await Task.Run(() => Console.WriteLine("dotnet-why"));
}