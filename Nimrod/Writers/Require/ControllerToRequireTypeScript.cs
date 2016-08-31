using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class ControllerToRequireTypeScript : ControllerToTypeScript
    {
        public ControllerToRequireTypeScript(Type type) : base(type) { }

        protected override IEnumerable<string> GetHeader()
        {
            var actions = TypeDiscovery.GetControllerActions(this.Type);

            var typesInParameters = actions.SelectMany(method => method.GetParameters().Select(p => p.ParameterType));
            var typesInReturns = actions.Select(method => method.GetReturnType());

            var importedTypes = typesInParameters.Union(typesInReturns).ToHashSet();

            var imports = RequireModuleHelper.GetTypesToImport(importedTypes)
                                             .Select(t => RequireModuleHelper.GetImportLine(t));

            return imports.Concat(new[] {
                $"import Nimrod = require('../Nimrod/Nimrod');",
                $"import IRestApi = require('./IRestApi');"
            });
        }

        protected override IEnumerable<string> GetFooter() => new string[0];
    }
}
