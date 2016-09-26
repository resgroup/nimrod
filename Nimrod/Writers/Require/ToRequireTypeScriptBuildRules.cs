using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Require
{
    public class ToRequireTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<TypeScriptType, bool, ToTypeScript> ControllerBuilder => (type, strictNullCheck) => new ControllerToRequireTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> EnumBuilder => (type, strictNullCheck) => new EnumToModuleTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> StructBuilder => (type, strictNullCheck) => new StructToRequireTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> ModelBuilder => (type, strictNullCheck) => new ModelToRequireTypeScript(type, strictNullCheck);
        public override StaticToTypeScript StaticBuilder => new StaticToRequireTypeScript();
    }
}
