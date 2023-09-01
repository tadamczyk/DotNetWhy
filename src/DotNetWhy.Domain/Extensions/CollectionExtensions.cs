namespace DotNetWhy.Domain.Extensions;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection) =>
        collection is null
        || !collection.Any();

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        var spanCollection = CollectionsMarshal.AsSpan(collection.ToList());

        foreach (var item in spanCollection) action(item);
    }
}