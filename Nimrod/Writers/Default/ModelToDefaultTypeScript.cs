using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Default
{
    public class ModelToDefaultTypeScript : ModelToTypeScript
    {
        public override bool PrefixPropertyWithNamespace => true;

        public ModelToDefaultTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        protected override IEnumerable<string> GetHeader()
        {
            var baseType = Type.Type.BaseType;
            bool hasExtension = baseType != null && !baseType.IsSystem();
            string extension = hasExtension ? $" extends {baseType.ToTypeScript().ToString(true, true, false)}" : "";

            return new[] {
                $"namespace {this.Type.Namespace} {{",
                $"export interface {TsName}{extension} {{"
            };
        }

        protected override IEnumerable<string> GetFooter() => new[] { "}", "}" };
    }
}
