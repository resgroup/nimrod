using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Require
{
    public class ToRequireTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<TypeScriptType, ToTypeScript> ControllerBuilder => type => new ControllerToRequireTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> EnumBuilder => type => new EnumToModuleTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> StructBuilder => type => new StructToRequireTypeScript(type);
        public override Func<TypeScriptType, ToTypeScript> ModelBuilder => type => new ModelToRequireTypeScript(type);
        public override StaticToTypeScript StaticBuilder => new StaticToRequireTypeScript();
    }
}
