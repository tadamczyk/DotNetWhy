namespace DotNetWhy.Domain.Providers;

internal interface ILockFileTargetProvider
{
    LockFileTarget Get(
        LockFile lockFile,
        string name);
}

internal sealed class LockFileTargetProvider : ILockFileTargetProvider
{
    public LockFileTarget Get(
        LockFile lockFile,
        string name) =>
        lockFile
            .Targets
            .FirstOrDefault(target => target.TargetFramework.ToString().Equals(name));
}