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

        public EnumToTypeScript(Type type) : base(type)
        {
            if (!this.Type.IsEnum)
            {
                throw new ArgumentException($"{this.Type.Name} is not an System.Enum.", nameof(type));
            }
            var underlyingType = Enum.GetUnderlyingType(this.Type);
            if (underlyingType != typeof(int))
            {
                throw new NotSupportedException( $"Unsupported underlying type for enums in typescript [{underlyingType}]. Only ints are supported.");
            }
        }

        public override IEnumerable<string> Build()
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
