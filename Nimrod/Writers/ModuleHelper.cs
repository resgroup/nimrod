using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public static class ModuleHelper
    {
        public static IEnumerable<Type> GetTypesToImport(IEnumerable<Type> types)
        {
            var baseTypes = types.Where(t => !t.IsGenericType);
            // when a type is generic, we want the template of it, not the particular template instance
            var genericTypes = types.Where(t => t.IsGenericType)
                .Select(t => t.GetGenericTypeDefinition());
            var referencedType = types
                .SelectMany(t => t.GetGenericArgumentsRecursively()
                .Select(arg => arg.IsGenericType ? arg.GetGenericTypeDefinition() : arg))
                .Distinct();

            return baseTypes.Union(genericTypes).Union(referencedType).Where(type => !type.IsSystem());
        }

        public static string GetImportLine(Type type, bool singleFile)
        {
            var tsType = type.ToTypeScript();
            var typeName = tsType.ToString(false, false, false);
            if (singleFile)
            {
                return $"import {typeName} from './{ tsType.TypeScriptModuleName}';";
            }
            else
            {
                return $"import {{ {typeName} }} from './{ type.Namespace}';";
            }
        }
    }
}

