using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Module
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToModuleTypeScript : ControllerToTypeScript
    {
        public ControllerToModuleTypeScript(Type type) : base(type) { }

        protected override IEnumerable<string> GetHeader()
        {
            var actions = TypeDiscovery.GetControllerActions(this.Type);
            var importedTypes = actions.Select(action => action.GetReturnTypeAndParameterTypes())
                                       .SelectMany(t => t)
                                       .Distinct();

            var imports = ModuleHelper.GetTypesToImport(importedTypes)
                                 .Select(t => ModuleHelper.GetImportLine(t));
            return imports.Union(new[] {
                $"import IRestApi from './IRestApi';",
                $"import IPromise from './IPromise';",
                $"import IRequestConfig from '../Nimrod/IRequestConfig';"
            });
        }

        protected override IEnumerable<string> GetFooter() => new[] {
            $"export default {GetControllerName()};"
        };
    }
}
