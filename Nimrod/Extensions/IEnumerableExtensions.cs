using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Return a list of lines indented, given the open and close context. default are braces '{' and '}'
        /// </summary>
        /// <param name="lines">The list of line to indent</param>
        /// <param name="indentString">The indentation, default is 4 spaces "    "</param>
        /// <param name="openContext">The open context char, default is '{'</param>
        /// <param name="closeContext">The close context char, default is '}'</param>
        /// <returns>The indented lines</returns>
        // Exceptions:
        //   T:System.InvalidOperationException:
        //     Cannot outdent, level is already at zero. There is a problem in the indentation problem.
        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines, string indentString = "    ", char openContext = '{', char closeContext = '}')
        {
            lines.ThrowIfNull(nameof(lines));

            int currentIndentation = 0;
            foreach (var line in lines)
            {
                var opened = line.Count(c => c == openContext);
                var closed = line.Count(c => c == closeContext);
                var lineIndentation = opened - closed;

                // outendation has to be done before writing
                if (lineIndentation < 0)
                {
                    if (currentIndentation + lineIndentation < 0)
                    {
                        throw new InvalidOperationException("Cannot outdent, level is already at zero. There is a problem in the indentation problem.");
                    }
                    currentIndentation += lineIndentation;
                }
                var indentArray = Enumerable.Range(0, currentIndentation).SelectMany(_ => indentString).ToArray();
                var indent = new string(indentArray);
                yield return $"{indent}{line}";

                // whereas indentation has to be done after writing
                if (lineIndentation > 0)
                {
                    currentIndentation += lineIndentation;
                }
            }
        }
    }
}
