namespace DotNetWhy.Services.Validators;

internal static class PackageNameValidator
{
    private static string ErrorMessage => "Package name was not specified.";

    public static bool IsValid(params object[] args)
    {
        if (string.IsNullOrWhiteSpace(args[0].ToString()))
        {
            Console.WriteLine(ErrorMessage);
            return false;
        }

        return true;
    }
}