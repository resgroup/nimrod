using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Default
{
    public class ToDefaultTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<TypeScriptType, bool, ToTypeScript> ControllerBuilder => (type, strictNullCheck) => new ControllerToDefaultTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> EnumBuilder => (type, strictNullCheck) => new EnumToDefaultTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> StructBuilder => (type, strictNullCheck) => new StructToDefaultTypeScript(type, strictNullCheck);
        public override Func<TypeScriptType, bool, ToTypeScript> ModelBuilder => (type, strictNullCheck) => new ModelToDefaultTypeScript(type, strictNullCheck);
        public override StaticToTypeScript StaticBuilder => new StaticToDefaultTypeScript();
    }
}
