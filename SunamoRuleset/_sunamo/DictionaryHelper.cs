namespace SunamoRuleset._sunamo;

internal class DictionaryHelper
{
    internal static void AddOrCreate<TKey, TValue, TCollectionElement>(IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value,
        bool isSkippingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDictionary = null) where TKey : notnull
    {
        var isComparingWithString = stringDictionary != null;
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

    internal static void AddOrCreate<TKey, TValue>(IDictionary<TKey, List<TValue>> dictionary, TKey key, TValue value,
        bool isSkippingDuplicateValues = false, Dictionary<TKey, List<string>>? stringDictionary = null) where TKey : notnull
    {
        AddOrCreate<TKey, TValue, object>(dictionary, key, value, isSkippingDuplicateValues, stringDictionary);
    }
}
