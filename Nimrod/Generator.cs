using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            var fileInfos = this.IoOperations.GetFileInfos(dllPaths);
            this.IoOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            var types = this.GetTypesToWrite(assemblies).ToList();
            var toWrites = this.GetDynamicContent(types, moduleType).Union(this.GetStaticContent(moduleType)).ToList();
            this.IoOperations.RecreateOutputFolder();

            this.IoOperations.WriteLog($"Writing {toWrites.Count} files...");
            toWrites.AsDebugFriendlyParallel().ForAll(content =>
            {
                this.IoOperations.WriteLog($"Writing {content.Name}...");
                this.IoOperations.WriteFile(content.Content, content.Name);
            });
            this.IoOperations.WriteLog($"Writing {types.Count} files...Done!");
        }

        private IEnumerable<FileToWrite> GetDynamicContent(IList<Type> types, ModuleType moduleType)
        {
            return types.AsDebugFriendlyParallel().Select(type =>
            {
                var buildRules = ToTypeScriptBuildRules.GetRules(moduleType);
                var toTypeScript = buildRules.GetToTypeScript(type);
                var lines = toTypeScript.GetLines();
                return new FileToWrite(GetTypeScriptFilename(type), lines);
            });
        }
        public static string GetTypeScriptFilename(Type type)
        {
            string name;
            if (type.IsGenericType)
            {
                var genericType = type.GetGenericTypeDefinition();
                name = genericType.Name.Remove(genericType.Name.IndexOf('`'));
            }
            else
            {
                name = type.Name;
            }

            if (type.IsController())
            {
                name = $"{type.Name.Replace("Controller", "Service")}";
            }

            return $"{type.Namespace}.{name}.ts";
        }
        private List<Type> GetTypesToWrite(IEnumerable<Assembly> assemblies)
        {
            this.IoOperations.WriteLog($"Discovering types..");
            var assemblyTypes = TypeDiscovery.GetControllers(assemblies)
                    .SelectMany(controller => TypeDiscovery.SeekTypesFromController(controller))
                    .ToList();

            // Write all types except the ones in System
            var toWrites = assemblyTypes
                .Where(t => !t.IsSystem())
                .Select(t => t.IsGenericType ? t.GetGenericTypeDefinition() : t)
                .Distinct()
                .ToList();
            return toWrites;
        }

        private IEnumerable<FileToWrite> GetStaticContent(ModuleType module)
        {
            var buildRules = ToTypeScriptBuildRules.GetRules(module);
            return new[] {
                new FileToWrite("IRestApi.ts", buildRules.StaticBuilder.GetRestApiLines()),
                new FileToWrite("IPromise.ts", buildRules.StaticBuilder.GetPromiseLines())
            };
        }
    }
}

