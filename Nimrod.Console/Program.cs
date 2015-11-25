using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;

namespace Nimrod.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var options = new Options();
                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    if (options.Help)
                    {
                        string helpText = CommandLine.Text.HelpText.AutoBuild(options).ToString();
                        System.Console.WriteLine(helpText);
                    }
                    else
                    {
                        var outputFolderPath = options.OutputPath;

                        var files = new List<FileInfo>();
                        foreach (var filePath in options.Files)
                        {
                            if (options.Verbose) System.Console.Write($"Reading file {filePath}...");

                            if (!File.Exists(filePath))
                            {
                                System.Console.WriteLine($"Warning! The specified file {filePath} doesn't exist and will be skipped.");
                            }
                            else
                            {
                                files.Add(new FileInfo(filePath));
                                if (options.Verbose) System.Console.WriteLine($"OK!");

                            }
                        }

                        System.Console.WriteLine("outputFolderPath = " + outputFolderPath);

                        var directories = files.Select(f => f.DirectoryName).Distinct();

                        // Load all assemblies in the folder
                        AssemblyLocator.Init();
                        foreach (var directory in directories)
                        {
                            foreach (var assemblyFile in Directory.EnumerateFiles(directory, "*.dll"))
                            {
                                if (options.Verbose) System.Console.Write($"Loading assembly {assemblyFile}");
                                Assembly.LoadFile(Path.Combine(directory, assemblyFile));
                                if (options.Verbose) System.Console.WriteLine($" Done!");
                            }
                        }

                        var foldersManager = new FoldersManager(outputFolderPath);
                        if (options.Verbose) System.Console.Write($"Recursive deletion of {outputFolderPath}...");
                        foldersManager.Recreate();
                        if (options.Verbose) System.Console.WriteLine($" Done!");


                        var assemblies = files.Select(t => Assembly.LoadFile(t.FullName));

                        System.Console.WriteLine($"Discovering types..");
                        var types = TypeDiscovery.GetControllerTypes(assemblies, true).ToList();
                        System.Console.WriteLine($" Done!");
                        // Write all types except the ones in System
                        var toWrites = types
                            .Where(t => t.IsSystem() == false)
                            .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                            .Distinct()
                            .ToList();
                        System.Console.Write($"Writing {toWrites.Count} files...");
                        toWrites.ForEach(t =>
                        {
                            System.Console.Write($"Writing {t.Name}...");
                            WriteType(foldersManager, t, options.ModuleType);
                            System.Console.WriteLine($" Done!");
                        });
                        System.Console.WriteLine($" Done!");
                    }
                }
                else
                {
                    System.Console.WriteLine($"Error in the command line arguments {string.Join(" ", args)}");
                    string helpText = CommandLine.Text.HelpText.AutoBuild(options).ToString();
                    System.Console.WriteLine(helpText);
                }

            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.ToString());
                Environment.Exit(-1);
            }
        }

        static private void WriteType(FoldersManager foldersManager, Type type, ModuleType module)
        {
            BaseWriter writer;

            string folder;
            if (type.IsWebMvcController())
            {
                writer = new ControllerWriter(module);
                folder = foldersManager.ControllersPath;
            }
            else if (type.IsEnum)
            {
                writer = new EnumWriter(module);
                folder = foldersManager.EnumsPath;
            }
            else if (type.IsValueType)
            {
                folder = foldersManager.SerializablesPath;
                writer = new SerializableWriter(module);
            }
            else
            {
                folder = foldersManager.ModelsPath;
                writer = new ModelWriter(module);
            }
            
            using (var fileWriter = File.CreateText(Path.Combine(folder, type.GetTypeScriptFilename())))
            {
                writer.Write(fileWriter, type);
            }
        }
    }
}
