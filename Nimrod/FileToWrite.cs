using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public class FileToWrite
    {
        public string Namespace { get; }
        public IEnumerable<string> Lines { get; }
        public string Content => this.Lines.IndentLines().Concat("").JoinNewLine();
        public string FileName => $"{Namespace}.ts";

        public FileToWrite(string @namespace, IEnumerable<string> lines)
        {
            this.Namespace = @namespace;
            this.Lines = lines.ThrowIfNull(nameof(lines));
        }
    }
}
