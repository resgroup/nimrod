using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public abstract class ToTypeScript
    {
        public TypeScriptType Type { get; }
        public bool StrictNullCheck { get; }
        public bool SingleFile { get; }
        public ToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
        {
            this.Type = type.ThrowIfNull(nameof(type));
            this.StrictNullCheck = strictNullCheck;
            this.SingleFile = singleFile;
        }
        public abstract IEnumerable<string> GetLines();
        public abstract IEnumerable<string> GetImports();
    }
}
