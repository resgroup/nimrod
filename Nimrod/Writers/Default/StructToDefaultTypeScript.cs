using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Default
{
    /// <summary>
    /// For classes which extends String, ie: which are Serializable
    /// </summary>
    public class StructToDefaultTypeScript : StructToTypeScript
    {
        public StructToDefaultTypeScript(Type type) : base(type)
        {
        }

        public override IEnumerable<string> GetLines()
        {
            var tsClassType = this.Type.ToTypeScript();
            yield return $"namespace {this.Type.Namespace} {{";
            yield return $"export class {tsClassType} extends String {"{}"}";
            yield return "}";
        }
    }
}
