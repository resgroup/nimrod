using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Writers.Require
{
    public class EnumToRequireTypeScript : EnumToTypeScript
    {
        public EnumToRequireTypeScript(Type type) : base(type)
        {

        }
        protected override IEnumerable<string> GetHeader()
        {
            yield return $"enum {TsName} {{";
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
            yield return $"export = {TsName};";
        }
    }
}
