using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nimrod
{
    public class Generator
    {
        public IoOperations IoOperations { get; }
        public Generator(IoOperations ioOperations)
        {
            this.IoOperations = ioOperations.ThrowIfNull(nameof(ioOperations));
        }

        public void Generate(IEnumerable<string> dllPaths, ModuleType moduleType)
        {
            var fileInfos = this.IoOperations.GetFileInfos(dllPaths).ToList();
            this.IoOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            var types = this.GetTypesToWrite(assemblies).ToList();
            var files = this.GetDynamicFiles(types, moduleType)
                 .Union(this.GetStaticFiles(moduleType))
                 .ToList();

            this.IoOperations.Dump(files);
        }
        private List<Type> GetTypesToWrite(IEnumerable<Assembly> assemblies)
        {
            this.IoOperations.WriteLog($"Discovering types..");
            var controllers = TypeDiscovery.GetWebControllers(assemblies).ToList();
            var assemblyTypes = controllers.SelectMany(TypeDiscovery.GetWebControllerActions)
                                           .SelectMany(MethodExtensions.GetReturnTypeAndParameterTypes)
                                           .ToList();
            var referencedTypes = TypeDiscovery.EnumerateTypes(assemblyTypes)
                                               .Union(assemblyTypes)
                                               .Union(controllers);

            // Write all types except the ones in System
            var toWrites = referencedTypes
                .Where(t => !t.IsSystem())
                .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                .Distinct()
                .ToList();
            return toWrites;
        }

        private IEnumerable<FileToWrite> GetDynamicFiles(IEnumerable<Type> types, ModuleType moduleType)
            => types.AsDebugFriendlyParallel().Select(type =>
            {
                var buildRules = ToTypeScriptBuildRules.GetRules(moduleType);
                var toTypeScript = buildRules.GetToTypeScript(type);
                var lines = toTypeScript.GetLines();
                return new FileToWrite(GetTypeScriptFilename(type), lines);
            });

        public static string GetTypeScriptFilename(Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                name = genericType.Name.Remove(genericType.Name.IndexOf('`'));
            }
            else if (type.IsWebController())
            {
                name = $"{type.Name.Replace("Controller", "Service")}";
            }
            else
            {
                name = type.Name;
            }
            return $"{type.Namespace}.{name}.ts";
        }


        private IEnumerable<FileToWrite> GetStaticFiles(ModuleType module)
        {
            var buildRules = ToTypeScriptBuildRules.GetRules(module);
            return new[] {
                new FileToWrite("IRestApi.ts", buildRules.StaticBuilder.GetRestApiLines()),
                new FileToWrite("IPromise.ts", buildRules.StaticBuilder.GetPromiseLines())
            };
        }
    }
}

