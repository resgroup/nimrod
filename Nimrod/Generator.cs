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
        public void Generate(IEnumerable<string> dllPaths, IoOperations ioOperations)
        {
            var fileInfos = ioOperations.GetFileInfos(dllPaths).ToList();
            ioOperations.LoadAssemblies(fileInfos);

            var assemblies = fileInfos.Select(t => Assembly.LoadFile(t.FullName));
            ioOperations.WriteLog($"Discovering types..");
            var types = GetTypesToWrite(assemblies).ToList();

            if (this.SingleFile)
            {
                var files = types.AsDebugFriendlyParallel().Select(GetFileToWrite).ToList();

                ioOperations.Dump(files);
            }
            else
            {
                var files = types.AsDebugFriendlyParallel().Select(type => new { Type = type, File = GetFileToWrite(type) })
                    .GroupBy(t => t.Type.Namespace)
                    .Select(a => new FileToWrite($"{a.Key}.ts", a.SelectMany(t => t.File.Lines), a.SelectMany(t => t.File.Imports).Distinct()))
                    .ToList();
                ioOperations.Dump(files);
            }

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

        private FileToWrite GetFileToWrite(Type type)
        {
            var toTypeScript = new ToTypeScriptBuildRules().GetToTypeScript(new TypeScriptType(type), this.StrictNullCheck, this.SingleFile);
            return new FileToWrite(GetTypeScriptFilename(type), toTypeScript.GetLines(), toTypeScript.GetImports());
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
