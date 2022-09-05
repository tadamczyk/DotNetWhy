namespace DotNetWhy.Core.Models;

internal record Retry(int Max = 3)
{
    private int _counter;

    public bool CanTryAgain() =>
        _counter++ < Max;
}