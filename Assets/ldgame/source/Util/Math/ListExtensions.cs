using System.Collections.Generic;
using UnityEngine;

public static class ListExtensions
{
    // Extension method for List<T> to get a random element
    public static T GetRandom<T>(this IList<T> list, bool ignoreEmpty = true)
    {
        if (list == null || list.Count == 0)
        {
            if (!ignoreEmpty)
                Debug.LogError("The list cannot be null or empty.");
            return default; // Return default value for type T (null for classes, default value for value types)
        }

        int index = UnityEngine.Random.Range(0, list.Count); // Use Unity's Random.Range
        return list[index];
    }
}