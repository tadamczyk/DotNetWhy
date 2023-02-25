namespace DotNetWhy.Core.Interfaces;

internal interface ILockFilesGenerator
{
    void Generate(string workingDirectory);
}