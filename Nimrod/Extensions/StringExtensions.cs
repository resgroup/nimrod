using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

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

        /// <summary>
        /// Return true if the string has whitespaces somewhere at the first embeded level
        /// </summary>
        public static bool HasNonEmbededWhiteSpace(this string text)
        {
            return text.RemoveBetween('(', ')')
                       .RemoveBetween('[', ']')
                       .RemoveBetween('{', '}')
                       .RemoveBetween('<', '>')
                       .Contains(" ");
        }

        /// <summary>
        /// http://stackoverflow.com/a/39096603/128662
        /// </summary>
        public static string RemoveBetween(this string rawString, char enter, char exit)
        {
            if (rawString.Contains(enter) && rawString.Contains(exit))
            {
                int substringStartIndex = rawString.IndexOf(enter) + 1;
                int substringLength = rawString.LastIndexOf(exit) - substringStartIndex;

                string replaced = rawString;
                if (substringLength > 0 && substringLength > substringStartIndex)
                {
                    string substring = rawString.Substring(substringStartIndex, substringLength).RemoveBetween(enter, exit);
                    if (substring.Length != substringLength) // This would mean that letters have been removed
                    {
                        replaced = rawString.Remove(substringStartIndex, substringLength).Insert(substringStartIndex, substring);
                    }
                }

                //Source: http://stackoverflow.com/a/1359521/3407324
                Regex regex = new Regex(string.Format("\\{0}.*?\\{1}", enter, exit));
                return new Regex(" +").Replace(regex.Replace(replaced, string.Empty), " ");
            }
            else
            {
                return rawString;
            }
        }
    }
}
