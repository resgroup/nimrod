using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class EnumWriter : BaseWriter
    {
        public EnumWriter(TextWriter writer, ModuleType moduleType) : base(writer, moduleType)
        {

        }

        public override void Write(Type type)
        {
            var underlyingType = Enum.GetUnderlyingType(type);
            var tsName = type.ToTypeScript();

            if (Module == ModuleType.TypeScript)
            {
                WriteLine(string.Format("module {0} {1}", type.Namespace, '{'));
                WriteLine($"export enum {tsName} {{");
            }
            else if (Module == ModuleType.Require)
            {
                WriteLine($"enum {tsName} {{");
            }

            foreach (var enumValue in type.GetEnumValues())
            {
                var enumName = type.GetEnumName(enumValue);
                if (underlyingType == typeof(int))
                {
                    WriteLine($"{enumName} = {(int)enumValue},");
                }
                else
                {
                    WriteLine($"// Unsupported type for enums in typescript [{underlyingType}]");
                }
            }

            WriteLine("}");

            if (Module == ModuleType.TypeScript)
            {
                WriteLine("}");
            }
            else if (Module == ModuleType.Require)
            {
                WriteLine($"export = {tsName};");
            }

        }

        public static string GetEnumDescription(object value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
