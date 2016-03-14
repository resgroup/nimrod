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
        public ControllerWriter(TextWriter writer, ModuleType moduleType) : base(writer, moduleType)
        {
        }

        public override void Write(Type type)
        {
            if (Module == ModuleType.TypeScript)
            {
                WriteLine(string.Format("module {0} {1}", type.Namespace, '{'));
            }
            else if (Module == ModuleType.Require)
            {
                WriteImports(type);
                WriteLine($"import Nimrod = require('../Nimrod/Nimrod');");
                WriteLine($"import IRestApi = require('./IRestApi');");
            }
            WriteInterface(type);
            WriteImplementation(type);
            if (Module == ModuleType.TypeScript)
            {
                WriteLine("}");
            }
        }

        public void WriteImports(Type controllerType)
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
            WriteImports(importedTypes);
        }


        public void WriteInterface(Type type)
        {
            WriteLine(string.Format("export interface I{0} {1}", type.Name.Replace("Controller", "Service"), '{'));

            foreach (var method in TypeDiscovery.GetControllerActions(type))
            {
                WriteMethodSignature(method);
                Write(";");
                WriteLine();
            }

            WriteLine("}");
        }

        public void WriteImplementation(Type type)
        {
            bool needNamespace = Module == ModuleType.TypeScript;
            var serviceName = type.Name.Replace("Controller", "Service");
            WriteLine($"export class {serviceName} implements I{serviceName} {{");

            foreach (var method in TypeDiscovery.GetControllerActions(type))
            {
                var httpVerb = method.FirstOrDefaultHttpMethodAttribute();
                var parameters = method.GetParameters().AsEnumerable();

                Write("public ");
                WriteMethodSignature(method);
                Write(" {");
                WriteLine();
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
                    WriteLine("(config || (config = {})).params = {");
                    foreach (var methodParameter in parameters)
                    {
                        WriteLine($"{methodParameter.Name}: {methodParameter.Name},");
                    }
                    WriteLine("};");
                    WriteLine($"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', config);");
                }
                else
                {
                    WriteLine("var data = {");
                    foreach (var methodParameter in parameters)
                    {
                        WriteLine($"{methodParameter.Name}: {methodParameter.Name},");
                    }
                    WriteLine("};");
                    WriteLine($"return restApi.{httpVerb}{genericArgString}('/{entityName}/{method.Name}', data, config);");
                }
                WriteLine("}");
            }

            WriteLine("}");
            if (Module == ModuleType.TypeScript)
            {
                WriteLine($"service('serverApi.{serviceName}', {serviceName});");
            }
        }

        private void WriteMethodSignature(MethodInfo method)
        {
            bool needNamespace = Module == ModuleType.TypeScript;
            Write(method.Name);

            if (Module == ModuleType.TypeScript)
            {
                Write("(restApi: Nimrod.IRestApi");
            }
            else
            {
                Write("(restApi: IRestApi");
            }
            foreach (var methodParameter in method.GetParameters())
            {
                Write(", ");
                Write(methodParameter.Name);
                Write(": ");
                Write(methodParameter.ParameterType.ToTypeScript(needNamespace));
            }
            Write(", config?: Nimrod.IRequestShortcutConfig)");
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
            Write($": Nimrod.IPromise<{returnType}>");
        }
    }
}
