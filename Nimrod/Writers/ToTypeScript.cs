using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public abstract class ToTypeScript
    {
        public abstract FileType FileType { get; }
        public TypeScriptType Type { get; }
        public bool StrictNullCheck { get; }
        public ToTypeScript(TypeScriptType type, bool strictNullCheck)
        {
            this.Type = type.ThrowIfNull(nameof(type));
            this.StrictNullCheck = strictNullCheck;
        }
        public abstract IEnumerable<string> GetLines();
        public abstract IEnumerable<Type> GetImports();
    }
}
