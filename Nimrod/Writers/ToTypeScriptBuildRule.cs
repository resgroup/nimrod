using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers
{
    public class ToTypeScriptBuildRule
    {
        public Func<TypeScriptType, bool> Predicate { get; }
        public Func<TypeScriptType, bool, ToTypeScript> Builder { get; }

        public ToTypeScriptBuildRule(Func<TypeScriptType, bool> predicate, Func<TypeScriptType, bool, ToTypeScript> builder)
        {
            this.Predicate = predicate;
            this.Builder = builder;
        }
    }
}
