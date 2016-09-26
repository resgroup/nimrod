using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Text;

namespace Nimrod.Console
{
    static public class OptionsHandler
    {
        static public int Handle(string[] args)
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
                    tracer.WriteLine($"Error in the command line arguments {args.Join(" ")}");
                    string helpText = CommandLine.Text.HelpText.AutoBuild(new Options()).ToString();
                    tracer.WriteLine(helpText);
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
        static private void OnOptionsSuccessful(Options options, TraceListener tracer)
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
                var generator = new Generator(options.StrictNullCheck, options.ModuleType);
                generator.Generate(options.Files, ioOperations);
            }
        }
    }
}
