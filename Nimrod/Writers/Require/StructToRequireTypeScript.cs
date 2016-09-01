using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class StructToRequireTypeScript : StructToTypeScript
    {
        public StructToRequireTypeScript(Type type) : base(type) { }

        public override IEnumerable<string> GetLines()
        {
            var tsClassType = this.Type.ToTypeScript();
            return new[] {
                $"class {tsClassType} extends String {{}}",
                $"export = {tsClassType};"
            };
        }
    }
}
