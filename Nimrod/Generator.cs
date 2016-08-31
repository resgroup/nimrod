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
            var contents = new[] {
                this.GetDynamicContent(types, moduleType),
                this.GetStaticContent(moduleType)
            }.SelectMany(t => t)
                .Select(file =>
                {
                    // add empty line, because it's prettier
                    var content = file.Item2.IndentLines().Concat(new[] { "" }).JoinNewLine();
                    return new
                    {
                        FileName = file.Item1,
                        Content = content
                    };
                })
                .ToList();
            this.IoOperations.RecreateOutputFolder();

            this.IoOperations.WriteLog($"Writing {contents.Count} files...");
            contents.AsParallel().ForAll(content =>
            {
                this.IoOperations.WriteLog($"Writing {content.FileName}...");
                this.IoOperations.WriteFile(content.Content, content.FileName);
            });
            this.IoOperations.WriteLog($"Writing {types.Count} files...Done!");
        }

        private IEnumerable<Tuple<string, IEnumerable<string>>> GetDynamicContent(IList<Type> types, ModuleType moduleType)
        {
            return types.AsParallel().Select(type =>
            {
                var buildRules = ToTypeScriptBuildRules.GetRules(moduleType);
                var toTypeScript = buildRules.GetToTypeScript(type);
                var lines = toTypeScript.GetLines();
                return Tuple.Create(type.GetTypeScriptFilename(), lines);
            });
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

        private IEnumerable<Tuple<string, IEnumerable<string>>> GetStaticContent(ModuleType module)
        {
            var buildRules = ToTypeScriptBuildRules.GetRules(module);
            {
                var lines = buildRules.StaticBuilder.GetRestApiLines();
                yield return Tuple.Create("IRestApi.ts", lines);
            }
            {
                var lines = buildRules.StaticBuilder.GetPromiseLines();
                yield return Tuple.Create("IPromise.ts", lines);
            }
        }
    }
}

