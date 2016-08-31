using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Default
{
    public class ModelToDefaultTypeScript : ModelToTypeScript
    {
        public override bool PrefixPropertyWithNamespace => true;

        public ModelToDefaultTypeScript(Type type) : base(type) { }

        protected override IEnumerable<string> GetHeader()
        {
            var baseType = Type.BaseType;
            bool hasExtension = baseType != null && !baseType.IsSystem();
            string extension = hasExtension ? $" extends {baseType.ToTypeScript(true)}" : "";

            return new[] {
                $"namespace {this.Type.Namespace} {{",
                $"export interface {TsName}{extension} {{"
            };
        }

        protected override IEnumerable<string> GetFooter() => new[] { "}", "}" };
    }
}
