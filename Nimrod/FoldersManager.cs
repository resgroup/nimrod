using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class FoldersManager
    {
        public string OutputFolderPath { get; }

        public string ControllersPath { get { return OutputFolderPath; } }
        public string ModelsPath { get { return OutputFolderPath; } }
        public string EnumsPath { get { return OutputFolderPath; } }
        public string SerializablesPath { get { return OutputFolderPath; } }

        public FoldersManager(string outputFolderPath)
        {
            OutputFolderPath = outputFolderPath;
        }

        public void Recreate()
        {
            if (Directory.Exists(OutputFolderPath))
            {
                Directory.Delete(OutputFolderPath, true);
            }
            Directory.CreateDirectory(OutputFolderPath);
            //Directory.CreateDirectory(Path.Combine(OutputFolderPath, "Controllers"));
            //Directory.CreateDirectory(Path.Combine(OutputFolderPath, "Models"));
            //Directory.CreateDirectory(Path.Combine(OutputFolderPath, "Enums"));
            //Directory.CreateDirectory(Path.Combine(OutputFolderPath, "Serializables"));
        }
    }
}
