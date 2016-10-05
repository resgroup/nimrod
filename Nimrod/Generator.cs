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
        public bool SingleFile { get; }
        public Generator(bool strictNullCheck, bool singleFile)
        {
            this.StrictNullCheck = strictNullCheck;
            this.SingleFile = singleFile;
        }
        public GeneratorResult Generate(IEnumerable<string> dllPaths, IoOperations ioOperations)
        {
            var fileInfos = ioOperations.GetFileInfos(dllPaths).ToList();
            ioOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            ioOperations.WriteLog($"Discovering types..");
            var types = GetTypesToWrite(assemblies).ToList();

            var stuff = types.AsDebugFriendlyParallel()
                             .Select(type => new { Type = type, File = GetFileToWrite(type) })
                             .ToList();
            IEnumerable<FileToWrite> files;
            if (this.SingleFile)
            {
                files = stuff.Select(t => t.File.Item2);
            }
            else
            {
                files = stuff
                   .GroupBy(t => t.Type.Namespace)
                   .Select(a => new FileToWrite($"{a.Key}.ts", a.SelectMany(t => t.File.Item2.Lines), a.SelectMany(t => t.File.Item2.Imports).Distinct()));
            }
            var controllers = stuff
                .Where(t => t.File.Item1 == FileType.Controller)
                                   .Select(t => t.File.Item2.Name)
                                   .ToList();
            var models = stuff
                   .Where(t => t.File.Item1 != FileType.Controller)
                  .Select(t => t.File.Item2.Name)
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

        private Tuple<FileType, FileToWrite> GetFileToWrite(Type type)
        {
            var toTypeScript = new ToTypeScriptBuildRules().GetToTypeScript(new TypeScriptType(type), this.StrictNullCheck, this.SingleFile);
            return Tuple.Create(toTypeScript.FileType, new FileToWrite(GetTypeScriptFilename(type), toTypeScript.GetLines(), toTypeScript.GetImports()));
        }

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
    }
}
