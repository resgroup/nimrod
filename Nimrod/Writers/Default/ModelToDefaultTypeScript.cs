using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod.Writers.Default
{
    public class ModelToDefaultTypeScript : ModelToTypeScript
    {
        public override bool PrefixPropertyWithNamespace => true;
        public ModelToDefaultTypeScript(Type type) : base(type)
        {

        }
        protected override IEnumerable<string> GetHeader()
        {
            yield return string.Format("module {0} {1}", this.Type.Namespace, '{');
            yield return $"export interface {TsName} {{";
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
            yield return "}";
        }
    }
}
