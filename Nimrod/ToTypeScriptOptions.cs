using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public class ToTypeScriptOptions
    {
        public bool IncludeNamespace { get; }
        public bool IncludeGenericArguments { get; }
        public bool StrictNullCheck { get; }

        public ToTypeScriptOptions()
        {
            IncludeNamespace = false;
            IncludeGenericArguments = true;
            StrictNullCheck = true;
        }
        public ToTypeScriptOptions(bool includeNamespace, bool includeGenericArguments, bool strictNullCheck)
        {
            this.IncludeNamespace = includeNamespace;
            this.IncludeGenericArguments = includeGenericArguments;
            this.StrictNullCheck = strictNullCheck;
        }

        public ToTypeScriptOptions WithIncludeNamespace(bool includeNamespace)
            => new ToTypeScriptOptions(includeNamespace, this.IncludeGenericArguments, this.StrictNullCheck);

        public ToTypeScriptOptions WithIncludeGenericArguments(bool includeGenericArguments)
            => new ToTypeScriptOptions(this.IncludeNamespace, includeGenericArguments, this.StrictNullCheck);

        public ToTypeScriptOptions WithStrictNullCheck(bool strictNullCheck)
            => new ToTypeScriptOptions(this.IncludeNamespace, this.IncludeGenericArguments, strictNullCheck);
    }
}
