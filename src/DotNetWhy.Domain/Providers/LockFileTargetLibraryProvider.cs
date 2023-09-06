namespace DotNetWhy.Domain.Providers;

internal interface ILockFileTargetLibraryProvider
{
    LockFileTargetLibrary Get(
        LockFileTarget lockFileTarget,
        string name);
}

internal sealed class LockFileTargetLibraryProvider : ILockFileTargetLibraryProvider
{
    public LockFileTargetLibrary Get(
        LockFileTarget lockFileTarget,
        string name) =>
        lockFileTarget
            .Libraries
            .FirstOrDefault(library => library.Name.Equals(name));
}