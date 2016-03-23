using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Abstractions;

namespace Nimrod.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    OnOptionsSuccessful(options);
                }
                else
                {
                    OnOptionsFailed(args);
                }
                return 0;
            }
            catch (Exception exception)
            {
                System.Console.WriteLine(exception.ToString());
                return -1;
            }
        }

        private static void OnOptionsFailed(string[] args)
        {
            System.Console.WriteLine($"Error in the command line arguments {string.Join(" ", args)}");
            string helpText = CommandLine.Text.HelpText.AutoBuild(new Options()).ToString();
            System.Console.WriteLine(helpText);
        }

        static void OnOptionsSuccessful(Options options)
        {
            if (options.Help)
            {
                string helpText = CommandLine.Text.HelpText.AutoBuild(options);
                System.Console.WriteLine(helpText);
            }
            else
            {
                var logger = options.Verbose ? new ConsoleLogger() : null;
                var ioOperations = new IOOperations(new FileSystem(), options.OutputPath, logger);
                var generator = new Generator(ioOperations);
                generator.Generate(options.Files, options.ModuleType);
            }
        }

    }
}
