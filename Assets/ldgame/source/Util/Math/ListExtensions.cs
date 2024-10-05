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
    
    // Extension method to shuffle a list
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Extension method to pop the last element from the list
    public static T Pop<T>(this IList<T> list)
    {
        if (list.Count == 0)
            throw new System.InvalidOperationException("Cannot pop from an empty list.");

        int lastIndex = list.Count - 1;
        T item = list[lastIndex];
        list.RemoveAt(lastIndex);
        return item;
    }
}