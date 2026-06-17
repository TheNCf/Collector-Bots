using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DictionaryExtensions
{
    public static TKey FindFirstKeyByValue<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, TValue targetValue)
    {
        var comparer = EqualityComparer<TValue>.Default;

        foreach (var pair in dictionary)
        {
            if (comparer.Equals(pair.Value, targetValue))
            {
                return pair.Key;
            }
        }

        return default;
    }
}