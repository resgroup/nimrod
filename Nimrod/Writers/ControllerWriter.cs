using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Nimrod
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerWriter : BaseWriter
    {
        public ControllerWriter(ModuleType moduleType) : base(moduleType)
        {
        }

        public override void Write(TextWriter writer, Type type)
        {
            if (Module == ModuleType.TypeScript)
            {
                WriteLine(writer, string.Format("module {0} {1}", type.Namespace, '{'));
                IncrementIndent();
            }
            else if (Module == ModuleType.Require)
            {
                WriteImports(writer, type);
                WriteLine(writer, $"import Nimrod = require('../Nimrod/Nimrod');");
                WriteLine(writer, $"import IRestApi = require('./IRestApi');");
            }
            WriteInterface(writer, type);
            WriteImplementation(writer, type);
            if (Module == ModuleType.TypeScript)
            {
                DecrementIndent();
                WriteLine(writer, "}");
            }
        }

        public static void WriteImports(TextWriter writer, Type controllerType)
        {
            var importedTypes = new HashSet<Type>();
            foreach (var method in TypeDiscovery.GetControllerActions(controllerType))
            {
                foreach (var parameter in method.GetParameters())
                {
                    importedTypes.Add(parameter.ParameterType);
                }
                foreach (var arg in method.ReturnType.GetGenericArguments())
                {
                    importedTypes.Add(arg);
                }
            }
            WriteImports(writer, importedTypes);
        }


        public void WriteInterface(TextWriter writer, Type type)
        {
            WriteLine(writer, string.Format("export interface I{0} {1}", type.Name.Replace("Controller", "Service"), '{'));
            IncrementIndent();

            foreach (var method in TypeDiscovery.GetControllerActions(type))
            {
                WriteIndent(writer);
                WriteMethodSignature(writer, method);
                Write(writer, ";");
                WriteLine(writer);
            }

            DecrementIndent();
            WriteLine(writer, "}");
        }

        public void WriteImplementation(TextWriter writer, Type type)
        {
            bool needNamespace = Module == ModuleType.TypeScript;
            var serviceName = type.Name.Replace("Controller", "Service");
            WriteLine(writer, $"export class {serviceName} implements I{serviceName} {{");
            IncrementIndent();

            foreach (var method in TypeDiscovery.GetControllerActions(type))
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters().AsEnumerable();

                WriteIndent(writer);
                Write(writer, "public ");
                WriteMethodSignature(writer, method);
                Write(writer, " {");
                WriteLine(writer);
                IncrementIndent();
                string genericArgString;
                var genericArguments = method.ReturnType.GetGenericArguments();
                if (genericArguments.Length == 0)
                {
                    genericArgString = "";
                }
                else
                {
                    genericArgString = $"<{genericArguments[0].ToTypeScript(needNamespace)}>";
                }

                var entityName = type.Name.Substring(0, type.Name.Length - "Controller".Length);

                if (httpVerb == HttpMethodAttribute.Get || httpVerb == HttpMethodAttribute.Delete)
                {
                    WriteLine(writer, "(config || (config = {})).params = {");
                    IncrementIndent();
                    foreach (var methodParameter in parameters)
                    {
                        WriteLine(writer, $"{methodParameter.Name}: {methodParameter.Name},");
                    }
                    DecrementIndent();
                    WriteLine(writer, "};");
                    WriteLine(writer, $"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', config);");
                }
                else
                {
                    WriteLine(writer, "var data = {");
                    IncrementIndent();
                    foreach (var methodParameter in parameters)
                    {
                        WriteLine(writer, $"{methodParameter.Name}: {methodParameter.Name},");
                    }
                    DecrementIndent();
                    WriteLine(writer, "};");
                    WriteLine(writer, $"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', data, config);");
                }
                DecrementIndent();
                WriteLine(writer, "}");
            }

            DecrementIndent();
            WriteLine(writer, "}");
            if (Module == ModuleType.TypeScript)
            {
                WriteLine(writer, $"service('serverApi.{serviceName}', {serviceName});");
            }
        }

        private void WriteMethodSignature(TextWriter writer, MethodInfo method)
        {
            bool needNamespace = Module == ModuleType.TypeScript;
            Write(writer, method.Name);

            if (Module == ModuleType.TypeScript)
            {
                Write(writer, "(restApi: Nimrod.IRestApi");
            }
            else
            {
                Write(writer, "(restApi: IRestApi");
            }
            foreach (var methodParameter in method.GetParameters())
            {
                Write(writer, ", ");
                Write(writer, methodParameter.Name);
                Write(writer, ": ");
                Write(writer, methodParameter.ParameterType.ToTypeScript(needNamespace));
            }
            Write(writer, ", config?: Nimrod.IRequestShortcutConfig)");
            string returnType;
            if (method.ReturnType.GetGenericArguments().Length == 1)
            {
                // the return type is generic, is should be something like Json<T>, so the promise will return a T
                var genericArguments = method.ReturnType.GetGenericArguments()[0];
                returnType = genericArguments.ToTypeScript(needNamespace);
            }
            else
            {
                // the return type is not wrapped, we can't determine it, so just a basic object
                returnType = "{}";
            }
            Write(writer, $": Nimrod.IPromise<{returnType}>");
        }
    }
}
