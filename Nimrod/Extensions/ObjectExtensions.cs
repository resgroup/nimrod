using System;
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

        public static T[] GetEnumValues<T>() where T : struct
        {
            return (T[])Enum.GetValues(typeof(T));
        }
    }
}
