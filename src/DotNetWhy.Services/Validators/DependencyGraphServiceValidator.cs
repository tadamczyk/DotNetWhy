namespace DotNetWhy.Services.Validators;

internal static class DependencyGraphServiceValidator
{
    private static string ErrorMessage => "Dependency graph service was not found.";

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