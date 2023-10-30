namespace DotNetWhy.Core.Results;

internal static class Extensions
{
    private const char ErrorsSplitter = ';';

    public static Result HandleWith(this IResultHandler handler, params IResultHandler[] nextHandlers)
    {
        var handlersResults = new[] {handler}
            .Concat(nextHandlers)
            .AsParallel()
            .Select(resultHandler => resultHandler.Handle())
            .ToArray();

        if (handlersResults.All(result => result.IsSuccess)) return handlersResults[0];

        var errors = handlersResults
            .Where(handlerResult => !handlerResult.IsSuccess)
            .Select(handlerResult => handlerResult.Error);

        var error = string.Join(ErrorsSplitter, errors);

        return Result.Failure(error);
    }

    public static Result<T> HandleNext<T>(this Result result, IResultHandler<T> nextHandler) =>
        result.IsSuccess
            ? nextHandler.Handle()
            : Result<T>.Failure(result.Error);

    public static Result<TV> Map<T, TV>(this Result<T> result, Func<T, TV> mapper) =>
        result.IsSuccess
            ? Result<TV>.Success(mapper(result.Value))
            : Result<TV>.Failure(result.Error);

    public static Result<T> Unwrap<T>(this Result<Result<T>> result) =>
        result.IsSuccess && result.Value.IsSuccess
            ? Result<T>.Success(result.Value.Value)
            : Result<T>.Failure(result.Error ?? result.Value.Error);

    public static Response ToResponse(this Result<Node> nodeResult) =>
        nodeResult.IsSuccess
            ? new Response(nodeResult.Value)
            : new Response(nodeResult.Error.Split(ErrorsSplitter));
}