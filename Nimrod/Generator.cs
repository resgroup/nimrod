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
        public ModuleType ModuleType { get; }
        public Generator(bool strictNullCheck, ModuleType moduleType)
        {
            this.StrictNullCheck = strictNullCheck;
            this.ModuleType = moduleType;
        }
        public void Generate(IEnumerable<string> dllPaths, IoOperations ioOperations)
        {
            var fileInfos = ioOperations.GetFileInfos(dllPaths).ToList();
            ioOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            ioOperations.WriteLog($"Discovering types..");
            var types = GetTypesToWrite(assemblies).ToList();
            var files = GetDynamicFiles(types.AsDebugFriendlyParallel())
                 .Union(GetStaticFiles())
                 .ToList();

            ioOperations.Dump(files);
        }
        private List<Type> GetTypesToWrite(IEnumerable<Assembly> assemblies)
        {
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

        private IEnumerable<FileToWrite> GetDynamicFiles(IEnumerable<Type> types)
                => types.Select(type => new FileToWrite(GetTypeScriptFilename(type),
                    ToTypeScriptBuildRules.GetRules(this.ModuleType).GetToTypeScript(new TypeScriptType(type), this.StrictNullCheck).GetLines())
                );

        static public string GetTypeScriptFilename(Type type)
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


        private IEnumerable<FileToWrite> GetStaticFiles()
        {
            var buildRules = ToTypeScriptBuildRules.GetRules(this.ModuleType);
            return new[] {
                new FileToWrite("IRestApi.ts", buildRules.StaticBuilder.GetRestApiLines()),
                new FileToWrite("IPromise.ts", buildRules.StaticBuilder.GetPromiseLines())
            };
        }
    }
}
