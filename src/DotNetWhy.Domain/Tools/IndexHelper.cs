namespace DotNetWhy.Domain;

public record IndexHelper
{
    public int Value { get; private set; }

    public int Next() => ++Value;

    public void Reset() => Value = 0;
}