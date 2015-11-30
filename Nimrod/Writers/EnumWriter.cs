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
        public EnumWriter(ModuleType module) : base(module)
        {

        }

        public override void Write(TextWriter writer, Type type)
        {
            var underlyingType = Enum.GetUnderlyingType(type);
            var tsName = type.ToTypeScript();

            if (Module == ModuleType.TypeScript)
            {
                WriteLine(writer, string.Format("module {0} {1}", type.Namespace, '{'));
                IncrementIndent();
                WriteLine(writer, $"export enum {tsName} {{");
            }
            else if (Module == ModuleType.Require)
            {
                WriteLine(writer, $"enum {tsName} {{");
            }

            IncrementIndent();

            foreach (var enumValue in type.GetEnumValues())
            {
                var enumName = type.GetEnumName(enumValue);
                if (underlyingType == typeof(int))
                {
                    WriteLine(writer, $"{enumName} = {(int)enumValue},");
                }
                else
                {
                    WriteLine(writer, $"// Unsupported type for enums in typescript [{underlyingType}]");
                }
            }

            DecrementIndent();
            WriteLine(writer, "}");

            if (Module == ModuleType.TypeScript)
            {
                DecrementIndent();
                WriteLine(writer, "}");
            }
            else if (Module == ModuleType.Require)
            {
                WriteLine(writer, $"export = {tsName};");
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
