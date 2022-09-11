namespace DotNetWhy.Domain;

public static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return collection is null || !collection.Any();
    }

    public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
    {
        foreach (var item in collection) action(item);
    }
}