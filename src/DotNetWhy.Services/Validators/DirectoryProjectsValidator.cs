namespace DotNetWhy.Services.Validators;

internal static class DirectoryProjectsValidator
{
    private static string ErrorMessage => "Directory {0} does not contain any C# project.";

    public static bool IsValid(params object[] args)
    {
        if (!((FileSystem) args[0]).Directory.GetFiles(args[1].ToString())
            .Any(f => f.EndsWith(".sln") || f.EndsWith(".csproj")))
        {
            Console.WriteLine(ErrorMessage, args[1]);
            return false;
        }

        return true;
    }
}