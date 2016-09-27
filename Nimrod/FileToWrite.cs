using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public class FileToWrite
    {
        public string Name { get; }
        public IEnumerable<string> Lines { get; }
        public IEnumerable<string> Imports { get; }
        public string Content => this.Imports.Concat(this.Lines).IndentLines().Concat("").JoinNewLine();
        public FileToWrite(string name, IEnumerable<string> lines, IEnumerable<string> imports)
        {
            this.Name = name;
            this.Lines = lines;
            this.Imports = imports;
        }
    }
}
