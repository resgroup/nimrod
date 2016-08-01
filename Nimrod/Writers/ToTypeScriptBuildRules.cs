using Nimrod.Writers.Default;
using Nimrod.Writers.Module;
using Nimrod.Writers.Require;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public abstract class ToTypeScriptBuildRules
    {
        public abstract Func<Type, ToTypeScript> ControllerBuilder { get; }
        public abstract Func<Type, ToTypeScript> EnumBuilder { get; }
        public abstract Func<Type, ToTypeScript> StructBuilder { get; }
        public abstract Func<Type, ToTypeScript> ModelBuilder { get; }
        public abstract StaticToTypeScript StaticBuilder { get; }

        public IEnumerable<ToTypeScriptBuildRule> Rules => new[] {
                new ToTypeScriptBuildRule(type => type.IsController(), ControllerBuilder),
                new ToTypeScriptBuildRule(type => type.IsEnum, EnumBuilder),
                new ToTypeScriptBuildRule(type => type.IsValueType, StructBuilder),
                new ToTypeScriptBuildRule(type => true, ModelBuilder)
            };
        public ToTypeScript GetToTypeScript(Type type)
        {
            var item = this.Rules
                    .Where(s => s.Predicate(type))
                    .Select(s => s.Builder(type))
                    .FirstOrDefault();
            if (item == null)
            {
                throw new NotImplementedException("Unable to build ToTypeScript object, object doesn't respect any rule");
            }
            return item;
        }

        internal static ToTypeScriptBuildRules GetRules(ModuleType moduleType)
        {
            switch (moduleType)
            {
                case ModuleType.Require: return new ToRequireTypeScriptBuildRules();
                case ModuleType.Module: return new ToModuleTypeScriptBuildRules();
                default: return new ToDefaultTypeScriptBuildRules();
            }
        }
    }
}
