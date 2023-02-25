namespace DotNetWhy.Domain;

public record RetryHelper(int Max = 3)
{
    public int Current { get; private set; }

    public bool CanTryAgain() => Current++ < Max;
}