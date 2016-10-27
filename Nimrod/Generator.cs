using Nimrod.Writers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public class Generator
    {
        public bool StrictNullCheck { get; }
        public Generator(bool strictNullCheck)
        {
            this.StrictNullCheck = strictNullCheck;
        }
        public GeneratorResult Generate(IEnumerable<string> dllPaths, IoOperations ioOperations)
        {
            var fileInfos = ioOperations.GetFileInfos(dllPaths).ToList();
            ioOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            ioOperations.WriteLog($"Discovering types..");
            var assembliesTypes = this.GetTypesToWrite(assemblies).ToList();

            var referencedTypes = TypeDiscovery.EnumerateTypes(assembliesTypes, t => !t.IsSystem());

            var types = referencedTypes
                // if a generic, get the type definition, not the actual implementation
                .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                .Distinct()
                .ToList();

            var toTypeScripts = types
                // do not write System types
                .Where(t => !t.IsSystem())
                .Select(type => new TypeScriptType(type))
                .Select(type => ToTypeScript.Build(type, this.StrictNullCheck))
                // an abstract controller should not be called directly by the api
                .Where(t => !t.IsAbstractController)
                .ToList();
            var files = toTypeScripts
                         .AsDebugFriendlyParallel()
                         .GroupBy(t => t.Type.Namespace)
                         .Select(grp => GetFileForNamespaceGroup(grp.Key, grp.ToList()));

            return new GeneratorResult(toTypeScripts, files.ToList());
        }

        private FileToWrite GetFileForNamespaceGroup(string @namespace, List<ToTypeScript> toTypeScripts)
        {
            bool containsControllers = toTypeScripts.Any(obj => obj.ObjectType == ObjectType.Controller);
            // add static import only if the files is going to contains controllers
            var extraImport = containsControllers ? new[] { $"import {{ RestApi, RequestConfig }} from '../Nimrod';" } : new string[0];
            var imports = toTypeScripts
                        .SelectMany(t => t.GetImports())
                        // do not write import for the same namespace
                        .Where(import => import.Namespace != @namespace)
                        .GroupBy(import => import.Namespace)
                        .Select(grp => $"import * as {grp.Key.Replace('.', '_')} from './{ grp.Key}';")
                        .OrderBy(importLine => importLine);
            var content = toTypeScripts.SelectMany(t => t.GetLines());
            return new FileToWrite($"{@namespace}", extraImport.Concat(imports).Concat(content));
        }

        /// <summary>
        /// Get every type of every method referenced by an action of a web controller.
        /// </summary>
        private List<Type> GetTypesToWrite(IEnumerable<Assembly> assemblies)
        {
            var controllers = TypeDiscovery.GetWebControllers(assemblies)
                    .Where(c => c.GetWebControllerActions().Any())
                    .ToList();
            var assemblyTypes = controllers.SelectMany(TypeDiscovery.GetWebControllerActions)
                                           .SelectMany(MethodExtensions.GetReturnTypeAndParameterTypes)
                                           .ToList();
            var result = assemblyTypes.Union(controllers).ToList();
            return result;
        }

    }
}
