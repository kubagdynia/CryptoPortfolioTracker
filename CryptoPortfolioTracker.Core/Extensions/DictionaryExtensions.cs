namespace CryptoPortfolioTracker.Core.Extensions;

public static class DictionaryExtensions
{
    public static void AddRange<T>(this ICollection<T> target, IEnumerable<T>? source)
    {
        ArgumentNullException.ThrowIfNull(target);

        if (source is null) return;

        foreach (var element in source)
        {
            target.Add(element);
        }
    }
}