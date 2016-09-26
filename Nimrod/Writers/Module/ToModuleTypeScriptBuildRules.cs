using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Module
{
    public class ToModuleTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<TypeScriptType, bool, ToTypeScript> ControllerBuilder => (type, strictNullCheck) => new ControllerToModuleTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> EnumBuilder => (type, strictNullCheck) => new EnumToModuleTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> StructBuilder => (type, strictNullCheck) => new StructToModuleTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> ModelBuilder => (type, strictNullCheck) => new ModelToModuleTypeScript(type, strictNullCheck);
        public override StaticToTypeScript StaticBuilder => new StaticToModuleTypeScript();
    }
}
