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
            var types = this.GetTypesToWrite(assemblies).ToList();

            var toTypeScritps = types.AsDebugFriendlyParallel()
                             .Select(type => new ToTypeScriptBuildRules().GetToTypeScript(new TypeScriptType(type), this.StrictNullCheck))
                             .ToList();
            var files = toTypeScritps
                   .GroupBy(t => t.Type.Namespace)
                   .Select(a =>
                   {
                       bool containsControllers = a.Any(aaa => aaa.FileType == FileType.Controller);
                       var extraImport = containsControllers ? new[] { $"import {{ RestApi, RequestConfig }} from '../Nimrod';" } : new string[0];
                       var imports = a.SelectMany(t => t.GetImports())
                                      .GroupBy(t => t.Namespace)
                                      .Where(t => t.Key != a.Key)
                                      .Select(grp => $"import * as {grp.Key.Replace('.', '_')} from './{ grp.Key}';");
                       var content = a.SelectMany(t => t.GetLines());
                       return new FileToWrite($"{a.Key}", extraImport.Concat(imports).Concat(content));
                   });

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
                                               .Union(controllers.Where(c => c.GetWebControllerActions().Any()));

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
