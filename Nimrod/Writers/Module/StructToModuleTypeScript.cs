using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers.Module
{
    public class StructToModuleTypeScript : StructToTypeScript
    {
        public StructToModuleTypeScript(Type type) : base(type)
        {
        }

        public override IEnumerable<string> GetLines()
        {
            var tsClassType = this.Type.ToTypeScript();
            yield return $"export default class {tsClassType} extends String {"{}"}";
        }
    }
}
