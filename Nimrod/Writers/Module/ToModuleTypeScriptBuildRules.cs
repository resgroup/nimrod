using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Module
{
    public class ToModuleTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<Type, ToTypeScript> ControllerBuilder => type => new ControllerToModuleTypeScript(type);
        public override Func<Type, ToTypeScript> EnumBuilder => type => new EnumToModuleTypeScript(type);
        public override Func<Type, ToTypeScript> StructBuilder => type => new StructToModuleTypeScript(type);
        public override Func<Type, ToTypeScript> ModelBuilder => type => new ModelToModuleTypeScript(type);
        public override StaticToTypeScript StaticBuilder => new StaticToModuleTypeScript();
    }
}
