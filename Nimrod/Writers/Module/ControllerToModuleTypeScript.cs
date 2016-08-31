using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Nimrod.Writers.Module
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToModuleTypeScript : ControllerToTypeScript
    {
        public ControllerToModuleTypeScript(Type type) : base(type)
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

                var returnType = method.GetReturnType();
                importedTypes.Add(returnType);
            }

            var imports = ModuleHelper.GetTypesToImport(importedTypes)
                                             .Select(t => ModuleHelper.GetImportLine(t));
            foreach (var import in imports)
            {
                yield return import;
            }
            yield return $"import IRestApi from './IRestApi';";
            yield return $"import IPromise from './IPromise';";
            yield return $"import IRequestConfig from '../Nimrod/IRequestConfig';";
        }


        protected override IEnumerable<string> GetFooter()
        {
            yield return $"export default {GetControllerName()};";
        }
    }
}
