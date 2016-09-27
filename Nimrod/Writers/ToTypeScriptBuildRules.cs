using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public class ToTypeScriptBuildRules
    {
        public IEnumerable<ToTypeScriptBuildRule> Rules => new[] {
                new ToTypeScriptBuildRule(type => type.Type.IsWebController(),(a, b, c) => new ControllerToTypeScript(a, b, c)),
                new ToTypeScriptBuildRule(type => type.Type.IsEnum, (a, b, c) => new EnumToTypeScript(a, b, c)),
                new ToTypeScriptBuildRule(type => type.Type.IsValueType, (a, b, c) => new StructToTypeScript(a, b, c)),
                new ToTypeScriptBuildRule(type => true, (a, b, c) => new ModelToTypeScript(a, b, c))
            };
        public ToTypeScript GetToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
        {
            var item = this.Rules
                    .Where(s => s.Predicate(type))
                    .Select(s => s.Builder(type, strictNullCheck, singleFile))
                    .FirstOrDefault();
            if (item == null)
            {
                throw new NotImplementedException("Unable to build ToTypeScript object, object doesn't respect any rule");
            }
            return item;
        }
    }
}
