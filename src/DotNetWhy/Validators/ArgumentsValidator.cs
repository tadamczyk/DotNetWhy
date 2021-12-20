namespace DotNetWhy.Validators;

internal static class ArgumentsValidator
{
    private static string ErrorMessage => "Package name was not specified.";

    public static bool IsValid(params object[] args)
    {
        if (args is null || !args.Any())
        {
            Console.WriteLine(ErrorMessage);
            return false;
        }

        return true;
    }
}