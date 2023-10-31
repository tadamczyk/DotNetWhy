namespace DotNetWhy.Core;

public sealed class Response
{
    internal Response(IEnumerable<string> errors)
    {
        IsSuccess = false;
        Errors = errors;
    }

    internal Response(Node node)
    {
        IsSuccess = true;
        Node = node;
    }

    public bool IsSuccess { get; }
    public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();
    public Node Node { get; }
}