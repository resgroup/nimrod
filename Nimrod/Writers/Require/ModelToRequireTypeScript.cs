using System;
using System.Collections.Generic;
using System.Linq;

namespace Nimrod.Writers.Require
{
    public class ModelToRequireTypeScript : ModelToTypeScript
    {

        public ModelToRequireTypeScript(Type type) : base(type)
        {
        }

        protected override IEnumerable<string> GetHeader()
        {
            var genericArguments = this.Type.GetGenericArguments().ToHashSet();
            var propertyTypes = this.Type.GetProperties()
                                    .Select(p => p.PropertyType)
                                    .Where(p => !genericArguments.Contains(p));
            var imports = RequireModuleHelper.GetTypesToImport(propertyTypes)
                                .Where(t => !genericArguments.Contains(t))
                                .Select(t => RequireModuleHelper.GetImportLine(t));

            return imports.Union(new[] {
                $"interface {TsName} {{"
            });
        }

        protected override IEnumerable<string> GetFooter() =>
            new[] {
                $"}}",
                $"export = { this.Type.ToTypeScript(false, false) };"
            };
    }
}
