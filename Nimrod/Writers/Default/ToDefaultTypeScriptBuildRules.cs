using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Default
{
    public class ToDefaultTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<TypeScriptType, ToTypeScript> ControllerBuilder => type => new ControllerToDefaultTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> EnumBuilder => type => new EnumToDefaultTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> StructBuilder => type => new StructToDefaultTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> ModelBuilder => type => new ModelToDefaultTypeScript(type);
        public override StaticToTypeScript StaticBuilder => new StaticToDefaultTypeScript();
    }
}
