using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod.Writers.Require
{
    public class ModelToRequireTypeScript : ModelToTypeScript
    {

        public ModelToRequireTypeScript(Type type) : base(type)
        {
        }

        protected override IEnumerable<string> GetHeader()
        {
            var genericArguments = new HashSet<Type>();
            if (this.Type.IsGenericTypeDefinition)
            {
                foreach (var t in this.Type.GetGenericArguments())
                {
                    genericArguments.Add(t);
                }
            }
            var propertyTypes = this.Type.GetProperties()
                                    .Select(p => p.PropertyType)
                                    .Where(p => !genericArguments.Contains(p));
            var imports = RequireModuleHelper.GetTypesToImport(propertyTypes)
                                .Where(t => !genericArguments.Contains(t))
                                .Select(t => RequireModuleHelper.GetImportLine(t));

            foreach (var import in imports)
            {
                yield return import;
            }
            yield return $"interface {TsName} {{";
        }

        protected override IEnumerable<string> GetFooter()
        {
            yield return "}";
            // no generic for export on require mode
            var nonGenericTypescriptClass = this.Type.ToTypeScript(false, false);
            yield return $"export = {nonGenericTypescriptClass};";
        }
    }
}
