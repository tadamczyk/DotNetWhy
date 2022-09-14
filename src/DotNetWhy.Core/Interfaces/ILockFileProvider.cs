namespace DotNetWhy.Core.Interfaces;

internal interface ILockFileProvider
{
    LockFile Get(string outputDirectory);
}