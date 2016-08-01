using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Require
{
    public class ToRequireTypeScriptBuildRules : ToTypeScriptBuildRules
    {
        public override Func<Type, ToTypeScript> ControllerBuilder => type => new ControllerToRequireTypeScript(type);
        public override Func<Type, ToTypeScript> EnumBuilder => type => new EnumToModuleTypeScript(type);
        public override Func<Type, ToTypeScript> StructBuilder => type => new StructToRequireTypeScript(type);
        public override Func<Type, ToTypeScript> ModelBuilder => type => new ModelToRequireTypeScript(type);
        public override StaticToTypeScript StaticBuilder => new StaticToRequireTypeScript();
    }
}
