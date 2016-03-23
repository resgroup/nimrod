using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    public class StructToRequireTypeScript : StructToTypeScript
    {
        public StructToRequireTypeScript(Type type) : base(type)
        {
        }

        public override IEnumerable<string> Build()
        {
            var tsClassType = this.Type.ToTypeScript();
            yield return $"class {tsClassType} extends String {"{}"}";
            yield return $"export = {tsClassType};";
        }
    }
}
