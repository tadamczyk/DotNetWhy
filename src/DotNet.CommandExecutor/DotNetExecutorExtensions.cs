namespace DotNet.CommandExecutor;

public static class DotNetExecutorExtensions
{
    public static DotNetExecutor InDirectory(this DotNetExecutor dotNetExecutor,
        string workingDirectory)
    {
        dotNetExecutor.SetPrivateFieldValue(DotNetExecutor.WorkingDirectoryFieldName,
            workingDirectory ?? Environment.CurrentDirectory);

        return dotNetExecutor;
    }

    public static DotNetExecutor WithArguments(this DotNetExecutor dotNetExecutor,
        IEnumerable<string> arguments)
    {
        dotNetExecutor.SetPrivateFieldValue(DotNetExecutor.ArgumentsFieldName,
            string.Join(" ", arguments ?? Array.Empty<string>()));

        return dotNetExecutor;
    }

    private static void SetPrivateFieldValue(this DotNetExecutor dotNetExecutor,
        string name,
        object value) =>
        dotNetExecutor
            .GetType()
            .GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)
            ?.SetValue(dotNetExecutor, value);
}