namespace DotNetWhy.Common.Extensions;

public static class CollectionExtensions
{
    public static void ForEach<TItem>(this IEnumerable<TItem> collection, Action<TItem> action)
    {
        var spanCollection = collection.ToArray().AsSpan();

        for (var iterator = 0; iterator < spanCollection.Length; iterator++) action(spanCollection[iterator]);
    }
}