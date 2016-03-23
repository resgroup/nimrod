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
            return string.Concat(Enumerable.Repeat(pattern, count));
        }
    }
}
