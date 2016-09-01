using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class ObjectExtensions
    {
        public static T ThrowIfNull<T>(this T obj, string objectName) where T : class
        {
            if (obj == null)
            {
                throw new ArgumentNullException(objectName, $"Argument {objectName} cannot be null.");
            }
            return obj;
        }

        public static T[] GetEnumValues<T>() where T : struct => (T[])Enum.GetValues(typeof(T));

        public static IEnumerable<T> SingleAsEnumerable<T>(T value) => new[] { value };


        public static Func<A, R> Memoize<A, R>(this Func<A, R> f) => Memoize(f, default(R));
        // http://stackoverflow.com/questions/1254995/thread-safe-memoization

        // use this method if you recursion might end up in a loop
        // provide an adhoc default like empty enumarable for loop results
        public static Func<A, R> Memoize<A, R>(this Func<A, R> f, R defaultR)
        {
            var cache = new ConcurrentDictionary<A, R>();
            var syncMap = new ConcurrentDictionary<A, object>();
            return a =>
            {
                R r;
                if (!cache.TryGetValue(a, out r))
                {
                    // custom modification: test if the parameter is already in syncmap
                    // this means we are in a loop, get out by returning the default value
                    if (syncMap.ContainsKey(a))
                    {
                        return defaultR;
                    }
                    else
                    {
                        var sync = syncMap.GetOrAdd(a, new object());
                        lock (sync)
                        {

                            r = cache.GetOrAdd(a, f);
                        }
                        syncMap.TryRemove(a, out sync);
                        return r;
                    }
                }
                else
                {
                    return r;
                }
            };
        }

    }
}
