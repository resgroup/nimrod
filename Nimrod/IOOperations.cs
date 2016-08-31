using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Abstractions;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public class IoOperations
    {
        public string OutputFolderPath { get; }
        public IFileSystem FileSystem { get; }
        public ILogger Logger { get; }

        public IoOperations(IFileSystem fileSystem, string outputFolderPath, ILogger logger)
        {
            this.FileSystem = fileSystem.ThrowIfNull(nameof(fileSystem));
            this.OutputFolderPath = outputFolderPath;
            this.Logger = logger.ThrowIfNull(nameof(logger)); ;

            this.WriteLog("outputFolderPath = " + outputFolderPath);
        }

        public void WriteLog(string log)
        {
            this.Logger.WriteLine(log);
        }

        public void RecreateOutputFolder()
        {
            this.WriteLog($"Recursive deletion of {this.OutputFolderPath}...");

            if (this.FileSystem.Directory.Exists(this.OutputFolderPath))
            {
                this.FileSystem.Directory.Delete(this.OutputFolderPath, true);
            }
            this.FileSystem.Directory.CreateDirectory(this.OutputFolderPath);
        }

        public IEnumerable<FileInfoBase> GetFileInfos(IEnumerable<string> filePaths)
        {
            var files = filePaths.Select(filePath =>
                {
                    this.WriteLog($"Reading dll {filePath}...");
                    if (this.FileSystem.File.Exists(filePath))
                    {
                        return new
                        {
                            File = this.FileSystem.FileInfo.FromFileName(filePath),
                            Success = true
                        };
                    }
                    else
                    {
                        this.WriteLog($"Warning! The specified dll {filePath} doesn't exist and will be skipped.");
                        return new
                        {
                            File = null as FileInfoBase,
                            Success = false
                        };
                    }
                })
                .Where(file => file.Success)
                .Select(file => file.File)
                .ToList();
            return files;
        }


        /// <summary>
        /// Load all assemblies found in the same folder as the given DLL
        /// </summary>
        /// <param name="files"></param>
        public void LoadAssemblies(IEnumerable<FileInfoBase> files)
        {
            var directories = files.Select(f => f.DirectoryName).Distinct();
            AssemblyLocator.Init();

            var assemblyPaths = directories.Select(directory =>
                this.FileSystem.Directory.EnumerateFiles(directory, "*.dll")
                    .Select(assemblyFile => this.FileSystem.Path.Combine(directory, assemblyFile))
            ).SelectMany(a => a).ToList();

            // this step is time consuming (around 1 second for 100 assemblies)
            // the parallelization doesn't seems to help much
            assemblyPaths.AsParallel().ForAll(assemblyPath =>
            {
                this.WriteLog($"Trying to load assembly {assemblyPath}...");
                var assembly = Assembly.LoadFile(assemblyPath);
                this.WriteLog($"Loaded {assembly.FullName}");
            });
        }

        /// <summary>
        /// Create a file with the content given in the output folder
        /// </summary>
        /// <param name="content"></param>
        /// <param name="fileName"></param>
        public void WriteFile(string content, string fileName)
        {
            var filePath = this.FileSystem.Path.Combine(this.OutputFolderPath, fileName);
            this.FileSystem.File.WriteAllText(filePath, content);
        }
    }
}
