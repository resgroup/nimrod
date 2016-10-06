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
        public IEnumerable<Type> Imports { get; }
        public string Content =>
            CustomImports.Concat(importLines).Concat(this.Lines).IndentLines().Concat("").JoinNewLine();

        public IEnumerable<string> CustomImports => new[] {
                $"import {{ RestApi, RequestConfig }} from '../Nimrod';",
        };

        public string FileName => $"{Namespace}.ts";

        IEnumerable<string> importLines => this.Imports.GroupBy(t => t.Namespace)
            .Where(t => t.Key != this.Namespace)
            .Select(grp => $"import * as {grp.Key.Replace('.', '_')} from './{ grp.Key}';");

        public FileToWrite(string @namespace, IEnumerable<string> lines, IEnumerable<Type> imports)
        {
            this.Namespace = @namespace;
            this.Lines = lines;
            this.Imports = imports;
        }
    }
}
