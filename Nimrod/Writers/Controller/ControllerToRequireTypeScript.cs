using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Nimrod
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToRequireTypeScript : ControllerToTypeScript
    {
        public ControllerToRequireTypeScript(Type type) : base(type)
        {
        }

        protected override IEnumerable<string> GetHeader()
        {
            var importedTypes = new HashSet<Type>();
            var actions = TypeDiscovery.GetControllerActions(this.Type);
            foreach (var method in actions)
            {
                // get types in parameter list
                foreach (var parameter in method.GetParameters())
                {
                    importedTypes.Add(parameter.ParameterType);
                }
                // get types in return type which has to be a generic type
                foreach (var arg in method.ReturnType.GetGenericArguments())
                {
                    importedTypes.Add(arg);
                }
            }

            var imports = RequireModuleWriter.GetImports(importedTypes);
            foreach (var import in imports)
            {
                yield return import;
            }
            yield return $"import Nimrod = require('../Nimrod/Nimrod');";
            yield return $"import IRestApi = require('./IRestApi');";
        }


        protected override IEnumerable<string> GetFooter()
        {
            return Enumerable.Empty<string>();
        }
    }
}
