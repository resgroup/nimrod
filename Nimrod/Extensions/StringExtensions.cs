﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class StringExtensions
    {
        public static string Repeat(this string pattern, int count) => Enumerable.Repeat(pattern, count).Join(string.Empty);

        /// <summary>
        /// Call string.Join with argument Environment.NewLine
        /// </summary>
        public static string JoinNewLine(this IEnumerable<string> values) => values.Join(Environment.NewLine);

        /// <summary>
        /// Call string.Join with argument separator
        /// </summary>
        public static string Join(this IEnumerable<string> values, string separator) => string.Join(separator, values);
    }
}
