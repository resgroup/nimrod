using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    // Memoization helper
    // use the overload with specific defaultValue to avoid stackoverflow exception
    // if you recursion has a loop in the stack call
    // provide an adhoc default like empty enumerable for loop results
    // No thread safe, do not call this in parallel
    public static class Memoization
    {
        public static Func<TKey, TValue> Memoize<TKey, TValue>(this Func<TKey, TValue> valueFactory)
            => Memoize(valueFactory, default(TValue));

        public static Func<TKey, TValue> Memoize<TKey, TValue>(this Func<TKey, TValue> valueFactory, TValue defaultValue)
        {
            var cache = new Dictionary<TKey, TValue>();
            var keysSeen = new HashSet<TKey>();
            return key =>
            {
                var seen = !keysSeen.Add(key);
                TValue value;
                if (!cache.TryGetValue(key, out value))
                {
                    if (seen)
                    {
                        // the value has been seen, but is not calculated (not in cache yet)
                        // so we are in a referenced loop
                        // avoid stackoverflow exception by returning something
                        value = defaultValue;
                    }
                    else
                    {
                        value = valueFactory(key);
                        cache.Add(key, value);
                    }
                }
                return value;
            };
        }
    }
}
