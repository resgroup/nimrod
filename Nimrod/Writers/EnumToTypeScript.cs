using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public abstract class EnumToTypeScript : ToTypeScript
    {
        public string TsName => this.Type.ToTypeScript();

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();
        protected abstract IEnumerable<string> GetHeaderDescription();
        protected abstract IEnumerable<string> GetFooterDescription();
        public EnumToTypeScript(Type type) : base(type)
        {
            if (!this.Type.IsEnum)
            {
                throw new ArgumentException($"{this.Type.Name} is not an System.Enum.", nameof(type));
            }
            var underlyingType = Enum.GetUnderlyingType(this.Type);
            if (underlyingType != typeof(int))
            {
                throw new NotSupportedException($"Unsupported underlying type for enums in typescript [{underlyingType}]. Only ints are supported.");
            }
        }

        public override IEnumerable<string> GetLines()
        {
            foreach (var line in this.GetHeader())
            {
                yield return line;
            }
            foreach (var line in this.GetBody())
            {
                yield return line;
            }
            foreach (var line in this.GetFooter())
            {
                yield return line;
            }

            foreach (var line in this.GetHeaderDescription())
            {
                yield return line;
            }
            foreach (var line in this.GetBodyDescription())
            {
                yield return line;
            }
            foreach (var line in this.GetFooterDescription())
            {
                yield return line;
            }
        }

        public IEnumerable<string> GetBodyDescription()
        {
            yield return $"static getDescription(item: {this.TsName}): string {{";
            yield return "switch (item) {";
            foreach (var enumValue in this.Type.GetEnumValues())
            {
                var description = GetEnumDescription(enumValue);
                var enumName = this.Type.GetEnumName(enumValue);
                yield return $"case {this.TsName}.{enumName}: return '{description}';";
            }
            yield return "default: return item.toString();";
            yield return "}";
            yield return "}";
        }
        public static string GetEnumDescription(object value)
        {
            var fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        public IEnumerable<string> GetBody()
        {
            foreach (var enumValue in this.Type.GetEnumValues())
            {
                var enumName = this.Type.GetEnumName(enumValue);
                yield return $"{enumName} = {(int)enumValue},";
            }
        }
    }
}
