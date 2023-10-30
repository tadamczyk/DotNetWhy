namespace DotNetWhy.Core.Results;

internal sealed class Result<T>
{
    private Result(bool isSuccess, T value, string error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
        Value = value;
    }

    public bool IsSuccess { get; }
    public string Error { get; }
    public T Value { get; }

    public static Result<T> Success(T value) =>
        new(true, value);

    public static Result<T> Failure(string error) =>
        new(false, default, error);
}