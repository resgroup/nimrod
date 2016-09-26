using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Default
{
    /// <summary>
    /// For classes which extends String, ie: which are Serializable
    /// </summary>
    public class StructToDefaultTypeScript : StructToTypeScript
    {
        public StructToDefaultTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        public override IEnumerable<string> GetLines() => new[] {
            $@"namespace {this.Type.Namespace} {{
                export class {this.Type} extends String {{}}
            }}"
        };
    }
}
