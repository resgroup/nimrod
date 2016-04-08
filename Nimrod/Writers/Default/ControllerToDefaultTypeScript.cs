using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Nimrod.Writers.Default
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToDefaultTypeScript : ControllerToTypeScript
    {
        public override bool NeedNameSpace => true;

        public ControllerToDefaultTypeScript(Type type) : base(type)
        {
        }

        protected override IEnumerable<string> GetHeader()
        {
            yield return $"namespace {this.Type.Namespace} {{";
        }


        protected override IEnumerable<string> GetFooter()
        {
            yield return $"service('serverApi.{ServiceName}', {ServiceName});";
            yield return "}";
        }
    }
}
