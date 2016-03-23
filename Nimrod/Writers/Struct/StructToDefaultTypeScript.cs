using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    /// <summary>
    /// For classes which extends String, ie: which are Serializable
    /// </summary>
    public class StructToDefaultTypeScript : StructToTypeScript
    {
        public StructToDefaultTypeScript(Type type) : base(type)
        {
        }

        public override IEnumerable<string> Build()
        {
            var tsClassType = this.Type.ToTypeScript();
            yield return $"module {this.Type.Namespace} {{";
            yield return $"export class {tsClassType} extends String {"{}"}";
            yield return "}";
        }
    }
}
