using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Module
{
    public class EnumToModuleTypeScript : EnumToTypeScript
    {
        public EnumToModuleTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        protected override IEnumerable<string> GetHeader() => new[] {
            $"enum {TsName} {{"
        };

        protected override IEnumerable<string> GetFooter() => new[] {
            $"}}"
        };

        protected override IEnumerable<string> GetHeaderDescription() => new[] {
            $"export default class {TsName}Utilities {{"
        };


        protected override IEnumerable<string> GetFooterDescription() => new[] {
            $"}}"
        };
    }
}
