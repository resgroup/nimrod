using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public abstract class ToTypeScript
    {
        public TypeScriptType Type { get; }
        public ToTypeScript(TypeScriptType type)
        {
            this.Type = type.ThrowIfNull(nameof(type));
        }
        public abstract IEnumerable<string> GetLines();
    }
}
