using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ModelWriter : BaseWriter
    {
        public ModelWriter(TextWriter writer, ModuleType moduleType) : base(writer, moduleType)
        {
        }

        public override void Write(Type type)
        {
            bool needNamespace = Module == ModuleType.TypeScript;
            var tsClassType = type.ToTypeScript(false);

            if (Module == ModuleType.TypeScript)
            {
                WriteLine(string.Format("module {0} {1}", type.Namespace, '{'));
                WriteLine($"export interface {tsClassType} {{");
            }
            else if (Module == ModuleType.Require)
            {
                var genericArguments = new HashSet<Type>();
                if (type.IsGenericTypeDefinition)
                {
                    foreach(var t in type.GetGenericArguments())
                    {
                        genericArguments.Add(t);
                    }
                }
                var propertyTypes = type.GetProperties()
                                        .Select(p => p.PropertyType)
                                        .Where(p => genericArguments.Contains(p) == false);
                WriteImports(propertyTypes, genericArguments);
                WriteLine($"interface {tsClassType} {{");
            }

            foreach (var property in type.GetProperties())
            {
                var attributes = Attribute.GetCustomAttributes(property);
                var ignoreDataMemberAttributes = attributes.Where(attribute => attribute.GetType() == typeof(IgnoreDataMemberAttribute));
                if (ignoreDataMemberAttributes.Any() == false)
                {
                    var tsPropertyType = property.PropertyType.ToTypeScript(needNamespace);
                    WriteLine($"{property.Name}: {tsPropertyType};");
                }
            }

            WriteLine("}");


            if (Module == ModuleType.TypeScript)
            {
                WriteLine("}");
            }
            else if (Module == ModuleType.Require)
            {
                // no generic for export on require mode
                var nonGenericTypescriptClass = type.ToTypeScript(false, false);
                WriteLine($"export = {nonGenericTypescriptClass};");
            }
        }
    }
}
