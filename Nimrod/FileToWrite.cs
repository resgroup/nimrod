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
        public string Content => this.Lines.IndentLines().Concat(new[] { "" }).JoinNewLine();
        public FileToWrite(string name, IEnumerable<string> lines)
        {
            this.Name = name;
            this.Lines = lines;
        }
    }
}
