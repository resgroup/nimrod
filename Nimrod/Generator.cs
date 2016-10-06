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
            var types = GetTypesToWrite(assemblies).ToList();

            var toTypeScritps = types.AsDebugFriendlyParallel()
                             .Select(type => new ToTypeScriptBuildRules().GetToTypeScript(new TypeScriptType(type), this.StrictNullCheck))
                             .ToList();
            var files = toTypeScritps
                   .GroupBy(t => t.Type.Namespace)
                   .Select(a => new FileToWrite($"{a.Key}", a.SelectMany(t => t.GetLines()), a.SelectMany(t => t.GetImports())));

            var controllers = toTypeScritps
                .Where(t => t.FileType == FileType.Controller)
                                   .Select(t => t.Type.Namespace)
                                   .ToList();
            var models = toTypeScritps
                   .Where(t => t.FileType != FileType.Controller)
                  .Select(t => t.Type.Namespace)
                  .ToList();


            return new GeneratorResult(controllers, models, files.ToList());

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

    }
}
