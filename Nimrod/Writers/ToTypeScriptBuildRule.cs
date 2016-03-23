using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class ToTypeScriptBuildRule
    {
        public Func<Type, ModuleType, bool> Predicate;
        public Func<Type, ToTypeScript> Builder;

        public ToTypeScriptBuildRule(Func<Type, ModuleType, bool> predicate, Func<Type, ToTypeScript> builder)
        {
            this.Predicate = predicate;
            this.Builder = builder;
        }


        public static ToTypeScript GetToTypeScript(Type type, ModuleType module)
        {
            var item = ToTypeScriptBuildRule.Rules()
                    .Where(s => s.Predicate(type, module))
                    .Select(s => s.Builder(type))
                    .FirstOrDefault();
            if (item == null)
            {
                throw new NotImplementedException("Unable to build ToTypeScript object, object doesn't respect any rule");
            }
            return item;
        }

        public static IEnumerable<ToTypeScriptBuildRule> Rules()
        {
            var rules = new[] {
                     new ToTypeScriptBuildRule((type, module) => type.IsWebMvcController() && module == ModuleType.Require,type =>new ControllerToRequireTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => type.IsWebMvcController() && module == ModuleType.TypeScript,type =>new ControllerToDefaultTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => type.IsEnum && module == ModuleType.Require,type =>new EnumToRequireTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => type.IsEnum && module == ModuleType.TypeScript,type =>new EnumToDefaultTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => type.IsValueType && module == ModuleType.Require,type =>new StructToRequireTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => type.IsValueType && module == ModuleType.TypeScript,type =>new StructToDefaultTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => module == ModuleType.TypeScript,type =>new ModelToRequireTypeScript(type)),
                     new ToTypeScriptBuildRule((type, module) => true,type =>new ModelToDefaultTypeScript(type))
            };

            return rules;
        }
    }
}
