using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public class ToTypeScriptBuildRules
    {
        public IEnumerable<ToTypeScriptBuildRule> Rules => new[] {
                new ToTypeScriptBuildRule(type => type.Type.IsWebController(),(type, b) => new ControllerToTypeScript(type, b)),
                new ToTypeScriptBuildRule(type => type.Type.IsEnum, (type, b) => new EnumToTypeScript(type, b)),
                new ToTypeScriptBuildRule(type => type.Type.IsValueType, (type, b) => new StructToTypeScript(type, b)),
                new ToTypeScriptBuildRule(type => true, (type, b) => new ModelToTypeScript(type, b))
            };
        public ToTypeScript GetToTypeScript(TypeScriptType type, bool strictNullCheck)
        {
            var item = this.Rules
                    .Where(s => s.Predicate(type))
                    .Select(s => s.Builder(type, strictNullCheck))
                    .FirstOrDefault();
            if (item == null)
            {
                throw new NotImplementedException("Unable to build ToTypeScript object, object doesn't respect any rule");
            }
            return item;
        }
    }
}
