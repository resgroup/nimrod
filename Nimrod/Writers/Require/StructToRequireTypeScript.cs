using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class StructToRequireTypeScript : StructToTypeScript
    {
        public StructToRequireTypeScript(TypeScriptType type) : base(type) { }

        public override IEnumerable<string> GetLines()
        {
            return new[] {
                $"class {this.Type} extends String {{}}",
                $"export = {this.Type};"
            };
        }
    }
}
