using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public abstract class ToTypeScript
    {
        public bool IsAbstractController => this.ObjectType == ObjectType.Controller && this.Type.Type.IsAbstract;
        public abstract ObjectType ObjectType { get; }
        public TypeScriptType Type { get; }
        public bool StrictNullCheck { get; }
        public ToTypeScript(TypeScriptType type, bool strictNullCheck)
        {
            this.Type = type.ThrowIfNull(nameof(type));
            this.StrictNullCheck = strictNullCheck;
        }
        public abstract IEnumerable<string> GetLines();
        public abstract IEnumerable<Type> GetImports();

        public static ToTypeScript Build(TypeScriptType type, bool strictNullCheck)
        {
            if (type.Type.IsWebController())
            {
                return new ControllerToTypeScript(type, strictNullCheck);
            }
            else if (type.Type.IsEnum)
            {
                return new EnumToTypeScript(type, strictNullCheck);
            }
            else if (type.Type.IsValueType)
            {
                return new StructToTypeScript(type, strictNullCheck);
            }
            else
            {
                return new ModelToTypeScript(type, strictNullCheck);
            }
        }
    }
}
