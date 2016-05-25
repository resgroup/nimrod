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
            yield return string.Format("namespace {0} {1}", this.Type.Namespace, '{');

            var baseType = Type.BaseType;
            if (baseType != null && !baseType.IsSystem())
            {
                yield return $"export interface {TsName} extends {baseType.ToTypeScript(true)} {{";
            }
            else
            {
                yield return $"export interface {TsName} {{";
            }
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
            yield return "}";
        }
    }
}
