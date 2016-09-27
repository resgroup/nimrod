using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod.Writers
{
    public class ModelToTypeScript : ToTypeScript
    {
        public virtual bool PrefixPropertyWithNamespace => false;
        public string TsName => this.Type.ToString(new ToTypeScriptOptions().WithNullable(false));

        public ModelToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
            : base(type, strictNullCheck, singleFile) { }

        public override IEnumerable<string> GetImports()
        {
            var genericArguments = this.Type.Type.GetGenericArguments().ToHashSet();
            var propertyTypes = this.Type.Type.GetProperties()
                                .Select(p => p.PropertyType)
                                .Where(p => !genericArguments.Contains(p));
            var imports = ModuleHelper.GetTypesToImport(propertyTypes)
                                .Where(t => this.SingleFile ? true : t.Namespace != this.Type.Namespace)
                                .Where(t => !genericArguments.Contains(t))
                                .Select(t => ModuleHelper.GetImportLine(t, this.SingleFile));

            return imports;
        }

        protected string GetHeader()
        {
            if (this.SingleFile)
            {
                return $"interface {TsName} {{";
            }
            else
            {
                return $"export interface {TsName} {{";
            }
        }

        protected IEnumerable<string> GetFooter()
        {
            var nonGenericTypescriptClass = this.Type.ToString(false, false, false);
            yield return "}";
            if (this.SingleFile)
            {
                yield return $"export default {nonGenericTypescriptClass};";
            }
        }

        private IEnumerable<string> GetBody() => this.Type.Type.GetProperties()
                    .Select(property => new
                    {
                        Attributes = Attribute.GetCustomAttributes(property),
                        Property = property
                    })
                    // do not write attribute that are not serialize throught the [IgnoreDataMember] attribute
                    .Where(a => a.Attributes.OfType<IgnoreDataMemberAttribute>().IsEmpty())
                    .Select(a =>
                    {
                        var nullable = !a.Attributes.OfType<JetBrains.Annotations.NotNullAttribute>().Any();
                        var attributeName = a.Attributes.OfType<DataMemberAttribute>().FirstOrDefault()?.Name;
                        string propertyName = string.IsNullOrWhiteSpace(attributeName) ? a.Property.Name : attributeName;
                        var options = new ToTypeScriptOptions().WithIncludeNamespace(PrefixPropertyWithNamespace)
                                                               .WithNullable(this.StrictNullCheck && nullable);
                        return $"{propertyName}: { a.Property.PropertyType.ToTypeScript().ToString(options)};";
                    });

        public override IEnumerable<string> GetLines() =>
            new[] { this.GetHeader() }
            .Concat(this.GetBody())
            .Concat(this.GetFooter());
    }
}
