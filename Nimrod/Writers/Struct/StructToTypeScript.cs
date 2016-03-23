using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    /// <summary>
    /// For classes which extends String, ie: which are Serializable
    /// </summary>
    public abstract class StructToTypeScript : ToTypeScript
    {
        public StructToTypeScript(Type type) : base(type)
        {
            if (!this.Type.IsValueType)
            {
                throw new ArgumentException($"{this.Type.Name} is not a Struct.", nameof(type));
            }
        }

        public abstract override IEnumerable<string> Build();
    }
}
