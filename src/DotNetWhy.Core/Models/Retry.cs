namespace DotNetWhy.Core.Models;

internal record Retry(int Max = 3)
{
    public int Current { get; private set; }
    public bool CanTryAgain() => Current++ < Max;
}