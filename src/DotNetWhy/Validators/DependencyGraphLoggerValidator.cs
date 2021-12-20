namespace DotNetWhy.Validators;

internal static class DependencyGraphLoggerValidator
{
    private static string ErrorMessage => "Dependency graph logger was not found.";

    public static bool IsValid(params object[] args)
    {
        if (args[0] is null)
        {
            Console.WriteLine(ErrorMessage);
            return false;
        }

        return true;
    }
}