using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class ModelToRequireTypeScript : ModelToTypeScript
    {

        public ModelToRequireTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck)
        {
        }

        protected override IEnumerable<string> GetHeader()
        {
            var genericArguments = this.Type.Type.GetGenericArguments().ToHashSet();
            var propertyTypes = this.Type.Type.GetProperties()
                                    .Select(p => p.PropertyType)
                                    .Where(p => !genericArguments.Contains(p));
            var imports = RequireModuleHelper.GetTypesToImport(propertyTypes)
                                .Where(t => !genericArguments.Contains(t))
                                .Select(t => RequireModuleHelper.GetImportLine(t));

            return imports.Concat(new[] {
                $"interface {TsName} {{"
            });
        }

        protected override IEnumerable<string> GetFooter() =>
            new[] {
                $"}}",
                $"export = { this.Type.ToString(false, false, false) };"
            };
    }
}
