using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public class Generator
    {
        protected IFileSystem fileSystem { get; }

        public Generator(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public bool Generate(IOptions options, string helpText)
        {
            try
            {
                //var options = new Options();
                //if (CommandLine.Parser.Default.ParseArguments(args, options))
                //{

                if (options.Help)
                {
                    options.WriteLine(helpText);
                }
                else
                {
                    var outputFolderPath = options.OutputPath;

                    var files = new List<FileInfoBase>();
                    foreach (var filePath in options.Files)
                    {
                        options.Write($"Reading file {filePath}...");

                        if (!this.fileSystem.File.Exists(filePath))
                        {
                            System.Console.WriteLine($"Warning! The specified file {filePath} doesn't exist and will be skipped.");
                        }
                        else
                        {
                            files.Add(this.fileSystem.FileInfo.FromFileName(filePath));
                            options.WriteLine($"OK!");

                        }
                    }

                    options.WriteLine("outputFolderPath = " + outputFolderPath);

                    var directories = files.Select(f => f.DirectoryName).Distinct();

                    // Load all assemblies in the folder
                    AssemblyLocator.Init();
                    foreach (var directory in directories)
                    {
                        foreach (var assemblyFile in this.fileSystem.Directory.EnumerateFiles(directory, "*.dll"))
                        {
                            options.Write($"Loading assembly {assemblyFile}");
                            Assembly.LoadFile(this.fileSystem.Path.Combine(directory, assemblyFile));
                            options.WriteLine($" Done!");
                        }
                    }

                    var foldersManager = new FoldersManager(outputFolderPath);
                    options.Write($"Recursive deletion of {outputFolderPath}...");
                    foldersManager.Recreate();
                    options.WriteLine($" Done!");


                    var assemblies = files.Select(t => Assembly.LoadFile(t.FullName));

                    options.Write($"Discovering types..");
                    var types = TypeDiscovery.GetControllerTypes(assemblies, true).ToList();
                    options.WriteLine($" Done!");
                    // Write all types except the ones in System
                    var toWrites = types
                        .Where(t => t.IsSystem() == false)
                        .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                        .Distinct()
                        .ToList();
                    options.Write($"Writing static files...");
                    WriteStaticFiles(foldersManager, options.ModuleType);
                    options.WriteLine($" Done!");

                    options.Write($"Writing {toWrites.Count} files...");
                    toWrites.ForEach(t =>
                    {
                        options.Write($"Writing {t.Name}...");
                        WriteType(foldersManager, t, options.ModuleType);
                        options.WriteLine($" Done!");
                    });
                    options.WriteLine($"Writing {toWrites.Count} files...Done!");
                }

                //else
                //{
                //    System.Console.WriteLine($"Error in the command line arguments {string.Join(" ", args)}");
                //    string helpText = CommandLine.Text.HelpText.AutoBuild(options).ToString();
                //    System.Console.WriteLine(helpText);
                //}

            }
            catch (Exception e)
            {
                options.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        private void WriteStaticFiles(FoldersManager foldersManager, ModuleType module)
        {
            var writer = new StaticWriter(module);
            using (var fileWriter = this.fileSystem.File.CreateText(this.fileSystem.Path.Combine(foldersManager.OutputFolderPath, "IRestApi.ts")))
            {
                writer.Write(fileWriter);
            }
        }

        private void WriteType(FoldersManager foldersManager, Type type, ModuleType module)
        {
            BaseWriter writer;

            if (type.IsWebMvcController())
            {
                writer = new ControllerWriter(module);
            }
            else if (type.IsEnum)
            {
                writer = new EnumWriter(module);
            }
            else if (type.IsValueType)
            {
                writer = new SerializableWriter(module);
            }
            else
            {
                writer = new ModelWriter(module);
            }

            using (var fileWriter = this.fileSystem.File.CreateText(this.fileSystem.Path.Combine(foldersManager.OutputFolderPath, type.GetTypeScriptFilename())))
            {
                writer.Write(fileWriter, type);
            }
        }
    }
}
