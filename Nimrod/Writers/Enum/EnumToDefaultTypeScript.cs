using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class EnumToDefaultTypeScript : EnumToTypeScript
    {
        public EnumToDefaultTypeScript(Type type) : base(type)
        {

        }

        protected override IEnumerable<string> GetHeader()
        {
            yield return $"module {this.Type.Namespace} {{";
            yield return $"export enum {this.TsName} {{";
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
            yield return "}";
        }
    }
}
