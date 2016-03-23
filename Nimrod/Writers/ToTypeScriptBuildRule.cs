using Nimrod.Writers.Default;
using Nimrod.Writers.Require;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class ToTypeScriptBuildRule
    {
        public Func<Type, bool> Predicate;
        public Func<Type, ToTypeScript> Builder;

        public ToTypeScriptBuildRule(Func<Type, bool> predicate, Func<Type, ToTypeScript> builder)
        {
            this.Predicate = predicate;
            this.Builder = builder;
        }
    }
}
