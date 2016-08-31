using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Require
{
    public class EnumToModuleTypeScript : EnumToTypeScript
    {
        public EnumToModuleTypeScript(Type type) : base(type) { }

        protected override IEnumerable<string> GetHeader() => new[] {
            $"enum {TsName} {{"
        };

        protected override IEnumerable<string> GetFooter() => new[] {
            $"}}",
            $"export = {TsName};"
        };

        protected override IEnumerable<string> GetHeaderDescription() => new[] {
            $"class {TsName}Utilities {{"
        };

        protected override IEnumerable<string> GetFooterDescription() => new[] {
            $"}}",
            $"export = {TsName}Utilities;"
        };
    }
}
