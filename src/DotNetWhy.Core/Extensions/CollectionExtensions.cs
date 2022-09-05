namespace DotNetWhy.Core.Extensions;

internal static class CollectionExtensions
{
    public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
    {
        return collection is null || !collection.Any();
    }

    public static void AddIf<T>(this ICollection<T> collection, Func<bool> predicate, T item)
    {
        if (predicate.Invoke())
        {
            collection.Add(item);
        }
    }

    public static void AddIf<T>(this ICollection<T> collection, Func<T, bool> predicate, T item)
    {
        if (predicate.Invoke(item))
        {
            collection.Add(item);
        }
    }
}