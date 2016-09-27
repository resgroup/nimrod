using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandLine;

namespace Nimrod.Console
{
    public class Options
    {
        // -n --strictNullCheck
        [Option('n', "strictNullCheck", Required = false, HelpText = "Appends '| not null' to properties as needed by typescript 2.0")]
        public bool StrictNullCheck { get; set; } = false;

        // -v --verbose
        [Option('v', "verbose", Required = false, HelpText = "Prints all messages to standard output.")]
        public bool Verbose { get; set; } = false;

        [Option('h', "help", Required = false, HelpText = "Output the help")]
        public bool Help { get; set; }

        [Option('o', "output", Required = false, HelpText = "Directory where files will be generated.")]
        public string OutputPath { get; set; }

        [Option('g', "group", Required = false, HelpText = "Group modules by namespace")]
        public bool Group { get; set; }

        // Assemblies to retrieve, -f --files
        [OptionList('f', "files", Required = true, Separator = ',', HelpText = "Specify files, separated by a comma. Example --files=bin\\Assembly1.dll,bin\\Assembly2.dll")]
        public IList<string> Files { get; set; }
    }
}
