using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public class ToTypeScriptOptions
    {
        public Predicate<Type> IncludeNamespace { get; }
        public bool IncludeGenericArguments { get; }
        public bool Nullable { get; }


        public ToTypeScriptOptions()
        {
            IncludeNamespace = (type) => false;
            IncludeGenericArguments = true;
            Nullable = true;
        }
        public ToTypeScriptOptions(Predicate<Type> includeNamespace, bool includeGenericArguments, bool nullable)
        {
            this.IncludeNamespace = includeNamespace;
            this.IncludeGenericArguments = includeGenericArguments;
            this.Nullable = nullable;
        }

        public ToTypeScriptOptions WithIncludeNamespace(Predicate<Type> includeNamespace)
            => new ToTypeScriptOptions(includeNamespace, this.IncludeGenericArguments, this.Nullable);

        public ToTypeScriptOptions WithIncludeGenericArguments(bool includeGenericArguments)
            => new ToTypeScriptOptions(this.IncludeNamespace, includeGenericArguments, this.Nullable);

        public ToTypeScriptOptions WithNullable(bool nullable)
            => new ToTypeScriptOptions(this.IncludeNamespace, this.IncludeGenericArguments, nullable);
    }
}
