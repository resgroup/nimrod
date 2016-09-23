using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public abstract class EnumToTypeScript : ToTypeScript
    {
        public string TsName => this.Type.ToString();

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();
        protected abstract IEnumerable<string> GetHeaderDescription();
        protected abstract IEnumerable<string> GetFooterDescription();
        public EnumToTypeScript(TypeScriptType type) : base(type)
        {
            if (!this.Type.Type.IsEnum)
            {
                throw new ArgumentException($"{this.Type.Name} is not an System.Enum.", nameof(type));
            }
            var underlyingType = Enum.GetUnderlyingType(this.Type.Type);
            if (underlyingType != typeof(int))
            {
                throw new NotSupportedException($"Unsupported underlying type for enums in typescript [{underlyingType}]. Only ints are supported.");
            }
        }

        public override IEnumerable<string> GetLines()
            => this.GetHeader()
                    .Concat(this.GetBody())
                    .Concat(this.GetFooter())
                    .Concat(this.GetHeaderDescription())
                    .Concat(this.GetBodyDescription())
                    .Concat(this.GetFooterDescription());

        public IEnumerable<string> GetBodyDescription() => new[] {
            $@"static getDescription(item: {this.TsName}): string {{
                switch (item) {{
                        {this.Type.Type.GetEnumValues()
                        .OfType<object>()
                        .Select(enumValue =>
                        {
                            var description = EnumExtensions.GetDescription(enumValue);
                            var enumName = this.Type.Type.GetEnumName(enumValue);
                            return $"case {this.TsName}.{enumName}: return '{description}';";
                        }).JoinNewLine()}
                }}
            }}"
        };

        public IEnumerable<string> GetBody() =>
            this.Type.Type.GetEnumValues().OfType<object>()
                     .Select(enumValue =>
                     {
                         var enumName = this.Type.Type.GetEnumName(enumValue);
                         return $"{enumName} = {(int)enumValue},";
                     });
    }
}
