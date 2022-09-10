namespace DotNetWhy.Core.Interfaces;

internal interface ILockFileProvider
{
    LockFile Get(
        string workingDirectory,
        string outputDirectory);
}