using System;
using System.Collections.Generic;
using System.Linq;

namespace Creative.System.Core
{
    public static class DictionaryExtensions
    {
        public static TValue AddOrUpdateItem<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
            else dict.Add(key, value);

            return value;
        }
        public static TValue AddItem<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            dict.Add(key, value);
            return value;
        }

        public static TValue TryGetValue<TValue, TKey>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryGetValue(key, out TValue value) ? value : default(TValue);
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }
    }
}