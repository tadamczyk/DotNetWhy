namespace DotNetWhy.Core.Interfaces;

internal interface ILockFileSourceProvider
{
    string Get(string outputDirectory);
}