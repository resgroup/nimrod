using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    /// <summary>
    /// Base class to write typescript file
    /// </summary>
    public static class RequireModuleWriter
    {

        public static IEnumerable<string> GetImports(IEnumerable<Type> types)
        {
            return GetImports(types, Enumerable.Empty<Type>());
        }
        public static IEnumerable<string> GetImports(IEnumerable<Type> types, IEnumerable<Type> exclude)
        {
            return types
                .SelectMany(type => type.ReferencedTypes().Concat(new[] { type.IsGenericType ? type.GetGenericTypeDefinition() : type }))
                .Distinct()
                .Where(type => !type.IsSystem())
                .Where(type => exclude.Contains(type) == false)
                .Select(type =>
                {
                    var typeName = type.ToTypeScript(false, false);
                    var moduleName = type.TypeScriptModuleName();
                    return $"import {typeName} = require('./{moduleName}');";
                });
        }
    }
}

