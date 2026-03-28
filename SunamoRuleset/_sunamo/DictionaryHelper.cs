namespace SunamoRuleset._sunamo;

/// <summary>
/// Helper class for dictionary operations with list values.
/// </summary>
internal class DictionaryHelper
{
    /// <summary>
    /// Adds a value to a dictionary where values are lists, creating the list if the key does not exist.
    /// Supports IList keys with sequence equality comparison.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the values in the list.</typeparam>
    /// <typeparam name="TCollectionElement">The element type for IList key comparison.</typeparam>
    /// <param name="dictionary">The dictionary to add to.</param>
    /// <param name="key">The key under which to add the value.</param>
    /// <param name="value">The value to add to the list.</param>
    /// <param name="isSkippingDuplicateValues">If true, skips adding duplicate values to the list.</param>
    /// <param name="stringDictionary">Optional parallel dictionary for string comparison of values.</param>
    internal static void AddOrCreate<TKey, TValue, TCollectionElement>(IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value,
        bool isSkippingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDictionary = null) where TKey : notnull
    {
        var isComparingWithString = false;
        if (stringDictionary != null) isComparingWithString = true;
        if (key is IList && typeof(TCollectionElement) != typeof(Object))
        {
            var keyAsList = key as IList<TCollectionElement>;
            var isContainingKey = false;
            foreach (var item in dictionary)
            {
                var dictionaryKey = item.Key as IList<TCollectionElement>;
                if (dictionaryKey!.SequenceEqual(keyAsList!)) isContainingKey = true;
            }
            if (isContainingKey)
            {
                foreach (var item in dictionary)
                {
                    var dictionaryKey = item.Key as IList<TCollectionElement>;
                    if (dictionaryKey!.SequenceEqual(keyAsList!))
                    {
                        if (isSkippingDuplicateValues)
                            if (item.Value.Contains(value))
                                return;
                        item.Value.Add(value);
                    }
                }
            }
            else
            {
                List<TValue> valueList = new();
                valueList.Add(value);
                dictionary.Add(key, valueList);
                if (isComparingWithString)
                {
                    List<string> stringValueList = new();
                    stringValueList.Add(value!.ToString()!);
                    stringDictionary!.Add(key, stringValueList);
                }
            }
        }
        else
        {
            var shouldAdd = true;
            lock (dictionary)
            {
                if (dictionary.ContainsKey(key))
                {
                    if (isSkippingDuplicateValues)
                    {
                        if (dictionary[key].Contains(value))
                            shouldAdd = false;
                        else if (isComparingWithString)
                            if (stringDictionary![key].Contains(value!.ToString()!))
                                shouldAdd = false;
                    }
                    if (shouldAdd)
                    {
                        var existingValues = dictionary[key];
                        if (existingValues != null) existingValues.Add(value);
                        if (isComparingWithString)
                        {
                            var existingStringValues = stringDictionary![key];
                            if (existingValues != null) existingStringValues.Add(value!.ToString()!);
                        }
                    }
                }
                else
                {
                    if (!dictionary.ContainsKey(key))
                    {
                        List<TValue> valueList = new();
                        valueList.Add(value);
                        dictionary.Add(key, valueList);
                    }
                    else
                    {
                        dictionary[key].Add(value);
                    }
                    if (isComparingWithString)
                    {
                        if (!stringDictionary!.ContainsKey(key))
                        {
                            List<string> stringValueList = new();
                            stringValueList.Add(value!.ToString()!);
                            stringDictionary.Add(key, stringValueList);
                        }
                        else
                        {
                            stringDictionary[key].Add(value!.ToString()!);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Adds a value to a dictionary where values are lists, creating the list if the key does not exist.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary key.</typeparam>
    /// <typeparam name="TValue">The type of the values in the list.</typeparam>
    /// <param name="dictionary">The dictionary to add to.</param>
    /// <param name="key">The key under which to add the value.</param>
    /// <param name="value">The value to add to the list.</param>
    /// <param name="isSkippingDuplicateValues">If true, skips adding duplicate values to the list.</param>
    /// <param name="stringDictionary">Optional parallel dictionary for string comparison of values.</param>
    internal static void AddOrCreate<TKey, TValue>(IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value,
        bool isSkippingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDictionary = null) where TKey : notnull
    {
        AddOrCreate<TKey, TValue, object>(dictionary, key, value, isSkippingDuplicateValues, stringDictionary);
    }
}
