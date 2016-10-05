using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public class EnumToTypeScript : ToTypeScript
    {
        public override FileType FileType => FileType.Enum;
        public string TsName => this.Type.ToString();
        public EnumToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
            : base(type, strictNullCheck, singleFile)
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
        public override IEnumerable<string> GetImports() => new List<string>();

        public override IEnumerable<string> GetLines()
            => new[] { this.SingleFile ? $"enum {TsName} {{" : $"export enum {TsName} {{" }
                    .Concat(this.GetBody())
                    .Concat(new[] { $"}}" })
                    .Concat(this.GetBodyDescription());

        public IEnumerable<string> GetBodyDescription() => new[] {
            $@"
        export{(this.SingleFile ? " default" : "")} class {TsName}Utilities {{
            static getDescription(item: {this.TsName}): string {{
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
