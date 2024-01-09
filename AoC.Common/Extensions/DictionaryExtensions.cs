namespace AoC.Common.Extensions;

public static class DictionaryExtensions
{
    public static void AddOrSet<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] = value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this IDictionary<T, int> dictionary, T key, int value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrReplaceIfGreaterThan<T>(this IDictionary<T, int> dictionary, T key, int value)
    {
        if (dictionary.TryGetValue(key, out int currentValue))
        {
            if (value > currentValue)
            {
                dictionary[key] = value;
            }
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<T>(this IDictionary<T, long> dictionary, T key, long value)
    {
        if (dictionary.ContainsKey(key))
        {
            dictionary[key] += value;
        }
        else
        {
            dictionary.Add(key, value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, IEnumerable<TValue> values)
    {
        foreach (var value in values)
        {
            dictionary.AddOrUpdate(key, value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out List<TValue>? list))
        {
            list.Add(value);
        }
        else
        {
            dictionary.Add(key, [value]);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> dictionary, TKey key, IEnumerable<TValue> values)
    {
        foreach (var value in values)
        {
            dictionary.AddOrUpdate(key, value);
        }
    }

    public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, HashSet<TValue>> dictionary, TKey key, TValue value)
    {
        if (dictionary.TryGetValue(key, out HashSet<TValue>? set))
        {
            set.Add(value);
        }
        else
        {
            dictionary.Add(key, [value]);
        }
    }

    public static void RemoveRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<TKey> valuesToRemove)
    {
        foreach (var valueToRemove in valuesToRemove)
        {
            dictionary.Remove(valueToRemove);
        }
    }
}