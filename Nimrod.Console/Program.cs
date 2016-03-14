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
                    return (new Generator(fileSystem: new FileSystem()).Generate(options, CommandLine.Text.HelpText.AutoBuild(options).ToString())) ? 0 : -1;
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

        //static private void WriteStaticFiles(FoldersManager foldersManager, ModuleType module)
        //{
        //    var writer = new StaticWriter(module);
        //    using (var fileWriter = File.CreateText(Path.Combine(foldersManager.OutputFolderPath, "IRestApi.ts")))
        //    {
        //        writer.Write(fileWriter);
        //    }
        //}
        //static private void WriteType(FoldersManager foldersManager, Type type, ModuleType module)
        //{
        //    BaseWriter writer;

        //    if (type.IsWebMvcController())
        //    {
        //        writer = new ControllerWriter(module);
        //    }
        //    else if (type.IsEnum)
        //    {
        //        writer = new EnumWriter(module);
        //    }
        //    else if (type.IsValueType)
        //    {
        //        writer = new SerializableWriter(module);
        //    }
        //    else
        //    {
        //        writer = new ModelWriter(module);
        //    }

        //    using (var fileWriter = File.CreateText(Path.Combine(foldersManager.OutputFolderPath, type.GetTypeScriptFilename())))
        //    {
        //        writer.Write(fileWriter, type);
        //    }
        //}
    }
}
