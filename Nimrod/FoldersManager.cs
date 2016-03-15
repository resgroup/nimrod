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
            this.OutputFolderPath = outputFolderPath;
        }

        public void Recreate()
        {
            if (Directory.Exists(this.OutputFolderPath))
            {
                Directory.Delete(this.OutputFolderPath, true);
            }
            Directory.CreateDirectory(this.OutputFolderPath);
        }
    }
}
