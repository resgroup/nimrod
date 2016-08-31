using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class IEnumerableExtensions
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return !source.Any();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) => new HashSet<T>(source);

        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines)
        {
            return lines.IndentLines("    ", '{', '}');
        }
        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines, string indentationString)
        {
            return lines.IndentLines(indentationString, '{', '}');
        }
        /// <summary>
        /// Return a list of lines indented, given the open and close context. default are braces '{' and '}'
        /// </summary>
        /// <param name="lines">The list of line to indent</param>
        /// <param name="indentationString">The indentation, default is 4 spaces "    "</param>
        /// <param name="openContext">The open context char, default is '{'</param>
        /// <param name="closeContext">The close context char, default is '}'</param>
        /// <returns>The indented lines</returns>
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Cannot outdent, level is already at zero. There is a problem in the indentation problem.
        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines, string indentationString, char openContext, char closeContext)
        {
            if (lines == null)
            {
                yield break;
            }

            int indentationLevel = 0;
            foreach (var line in lines)
            {
                var opened = line.Count(c => c == openContext);
                var closed = line.Count(c => c == closeContext);
                var lineIndentationLevel = opened - closed;

                // outendation has to be done before writing
                if (lineIndentationLevel < 0)
                {
                    indentationLevel += lineIndentationLevel;
                    if (indentationLevel < 0)
                    {
                        throw new InvalidOperationException("Cannot outdent, level is already at zero. There is a problem in the indentation process.");
                    }
                }
                var indentation = indentationString.Repeat(indentationLevel);
                yield return $"{indentation}{line}";

                // whereas indentation has to be done after writing
                if (lineIndentationLevel > 0)
                {
                    indentationLevel += lineIndentationLevel;
                }
            }
        }
    }
}
