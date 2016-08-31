using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Default
{
    public class EnumToDefaultTypeScript : EnumToTypeScript
    {
        public EnumToDefaultTypeScript(Type type) : base(type) { }

        protected override IEnumerable<string> GetHeader() => new[] {
            $"namespace {this.Type.Namespace} {{",
            $"export enum {this.TsName} {{"
        };

        protected override IEnumerable<string> GetFooter() => new[] {
            $"}}",
            $"}}"
        };

        protected override IEnumerable<string> GetHeaderDescription() => new[] {
            $"namespace {this.Type.Namespace} {{",
            $"export class {this.TsName}Utilities {{"
        };

        protected override IEnumerable<string> GetFooterDescription() => new[] {
            $"}}",
            $"}}"
        };
    }
}
