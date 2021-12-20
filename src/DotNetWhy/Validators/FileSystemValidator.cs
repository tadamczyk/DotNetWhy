namespace DotNetWhy.Validators;

internal static class FileSystemValidator
{
    private static string ErrorMessage => "File system service was not found.";

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