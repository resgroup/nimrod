using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Module
{
    public class StructToModuleTypeScript : StructToTypeScript
    {
        public StructToModuleTypeScript(TypeScriptType type) : base(type) { }

        public override IEnumerable<string> GetLines() => new[] {
            $"export default class {this.Type} extends String {{}}"
        };
    }
}
