﻿namespace DotNetWhy.Domain;

public static class DependencyExtensions
{
    public static Dependency ToDependency(this LockFileTargetLibrary lockFileTargetLibrary) =>
        new(lockFileTargetLibrary.Name, lockFileTargetLibrary.Version.ToString());
}