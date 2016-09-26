using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class StructToRequireTypeScript : StructToTypeScript
    {
        public StructToRequireTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        public override IEnumerable<string> GetLines()
        {
            return new[] {
                $"class {this.Type} extends String {{}}",
                $"export = {this.Type};"
            };
        }
    }
}
