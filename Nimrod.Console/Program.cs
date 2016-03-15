using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
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
                    var generator = new Generator(fileSystem: new FileSystem());
                    string helpText = CommandLine.Text.HelpText.AutoBuild(options);
                    generator.Generate(options, helpText);
                    return 0;
                }
                else
                {
                    System.Console.WriteLine($"Error in the command line arguments {string.Join(" ", args)}");
                    string helpText = CommandLine.Text.HelpText.AutoBuild(options).ToString();
                    System.Console.WriteLine(helpText);
                    return 0;
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                return -1;
            }
        }
    }
}
