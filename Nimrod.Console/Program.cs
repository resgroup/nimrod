using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;
using System.Diagnostics;

namespace Nimrod.Console
{
    static class Program
    {
        static int Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var tracer = new ConsoleTraceListener();
            try
            {
                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    OnOptionsSuccessful(options, tracer);
                }
                else
                {
                    OnOptionsFailed(args, tracer);
                }
                tracer.WriteLine($"Runs in {stopwatch.ElapsedMilliseconds}ms");
                return 0;
            }
            catch (Exception exception)
            {
                tracer.WriteLine(exception.ToString());
                return -1;
            }
        }

        private static void OnOptionsFailed(string[] args, TraceListener tracer)
        {
            tracer.WriteLine($"Error in the command line arguments {args.Join(" ")}");
            string helpText = CommandLine.Text.HelpText.AutoBuild(new Options()).ToString();
            tracer.WriteLine(helpText);
        }

        static void OnOptionsSuccessful(Options options, TraceListener tracer)
        {
            if (options.Help)
            {
                string helpText = CommandLine.Text.HelpText.AutoBuild(options);
                tracer.WriteLine(helpText);
            }
            else
            {
                ILogger logger;
                if (options.Verbose)
                {
                    logger = new DateTimeLogger(tracer);
                }
                else
                {
                    logger = VoidLogger.Default;
                }
                var ioOperations = new IoOperations(new FileSystem(), options.OutputPath, logger);
                var generator = new Generator(ioOperations);
                generator.Generate(options.Files, options.ModuleType);
            }
        }
    }
}
