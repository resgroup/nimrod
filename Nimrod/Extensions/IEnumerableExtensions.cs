﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{


    public static class IEnumerableExtensions
    {

        public static IEnumerable<TOutput> ApplyTryGet<TSource, TOutput>(this IEnumerable<TSource> source, TryGetFunc<TSource, TOutput> outputSelector)
            => source.Select(item =>
            {
                TOutput output;
                bool success = outputSelector(item, out output);
                return new { Success = success, Output = output };
            }).Where(a => a.Success).Select(a => a.Output);

        public static TSource? FirstOrNullable<TSource>(this IEnumerable<TSource> source)
            where TSource : struct
            => source.Any() ? source.FirstOrDefault() : new TSource?();

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


        /// <summary>
        /// Return the source enumerable concatenate with the single item
        /// </summary>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> source, T item) => source.Concat(new[] { item });

        /// <summary>
        /// Return the source enumerable union with the single item
        /// </summary>
        public static IEnumerable<T> Union<T>(this IEnumerable<T> source, T item) => source.Union(new[] { item });

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
                .SelectMany(line => line.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None))
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
                        throw new InvalidOperationException($@"
Cannot outdent, level is already at zero. 
There is a problem in the indentation process.
Line is [{line}]");
                    }
                    return new
                    {
                        Level = previous.Level + line.Level,
                        Result = previous.Result.Concat($"{indentationString.Repeat(appliedIndentation)}{line.Line}")
                    };
                }, t => t.Result);

    }
}
