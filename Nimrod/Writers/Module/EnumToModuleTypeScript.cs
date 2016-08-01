using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Module
{
    public class EnumToModuleTypeScript : EnumToTypeScript
    {
        public EnumToModuleTypeScript(Type type) : base(type)
        {

        }
        protected override IEnumerable<string> GetHeader()
        {
            yield return $"enum {TsName} {{";
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
        }

        protected override IEnumerable<string> GetHeaderDescription()
        {
            yield return $"export default class {TsName}Utilities {{";
        }

        protected override IEnumerable<string> GetFooterDescription()
        {
            yield return "}";
        }
    }
}
