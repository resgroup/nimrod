using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class StringExtensions
    {
        public static string Repeat(this string pattern, int count)
        {
            var strings = Enumerable.Repeat(pattern, count);
            var join = string.Join(string.Empty, strings);
            return join;
        }
    }
}
