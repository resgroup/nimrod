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
        }
    }
}
