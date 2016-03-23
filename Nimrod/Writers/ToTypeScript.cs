using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public abstract class ToTypeScript
    {
        public Type Type { get; }
        public ToTypeScript(Type type)
        {
            this.Type = type.ThrowIfNull(nameof(type));
        }
        public abstract IEnumerable<string> Build();
    }
}
