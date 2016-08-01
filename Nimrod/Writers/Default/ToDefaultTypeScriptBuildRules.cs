using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Default
{
    public class ToDefaultTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<Type, ToTypeScript> ControllerBuilder => type => new ControllerToDefaultTypeScript(type);
        public override Func<Type, ToTypeScript> EnumBuilder => type => new EnumToDefaultTypeScript(type);
        public override Func<Type, ToTypeScript> StructBuilder => type => new StructToDefaultTypeScript(type);
        public override Func<Type, ToTypeScript> ModelBuilder => type => new ModelToDefaultTypeScript(type);
        public override StaticToTypeScript StaticBuilder => new StaticToDefaultTypeScript();
    }
}
