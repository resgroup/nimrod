using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// Set the Degree of parallelisme to 1 if in debug
        /// </summary>
        public static ParallelQuery<TSource> AsDebugFriendlyParallel<TSource>(this IEnumerable<TSource> source)
        {
            var query = source.AsParallel();
#if DEBUG
            query = query.WithDegreeOfParallelism(1);
#endif
            return query;
        }

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item) => source.Concat(new[] { item });

        public static bool IsEmpty<T>(this IEnumerable<T> source) => !source.Any();

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source) => new HashSet<T>(source);

        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines)
            => lines.IndentLines("    ", '{', '}');

        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines, string indentationString)
            => lines.IndentLines(indentationString, '{', '}');

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
        //     Cannot outdent, level is already at zero. There is a problem in the indentation process.
        public static IEnumerable<string> IndentLines(this IEnumerable<string> lines, string indentationString, char openContext, char closeContext)
                => lines
                // split lines based on standard new line for both unix and windows
                .SelectMany(line => line.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None))
                .Select(line => new
                {
                    Line = line.Trim(),
                    Level = line.Count(c => c == openContext) - line.Count(c => c == closeContext)
                })
                .Aggregate(new { Level = 0, Result = new string[0].AsEnumerable() }, (previous, line) =>
                {
                    // the applied indentation depend if this is an indentation or an outdentation
                    var appliedIndentation = previous.Level + (line.Level < 0 ? line.Level : 0);
                    if (appliedIndentation < 0)
                    {
                        string message = $@"Cannot outdent, level is already at zero. 
There is a problem in the indentation process.
Line is [{line}]";
                        throw new InvalidOperationException(message);
                    }
                    return new
                    {
                        Level = previous.Level + line.Level,
                        Result = previous.Result.Concat($"{indentationString.Repeat(appliedIndentation)}{line.Line}")
                    };
                }, t => t.Result);

    }
}
