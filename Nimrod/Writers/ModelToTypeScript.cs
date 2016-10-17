using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod.Writers
{
    public class ModelToTypeScript : ToTypeScript
    {
        public override ObjectType ObjectType => ObjectType.Model;

        public ModelToTypeScript(TypeScriptType type, bool strictNullCheck)
            : base(type, strictNullCheck) { }

        public override IEnumerable<Type> GetImports()
        {
            var genericArguments = this.Type.Type.GetGenericArguments().ToHashSet();
            var propertyTypes = this.Type.Type.GetProperties()
                                .Select(p => p.PropertyType)
                                .Where(p => !genericArguments.Contains(p));
            var imports = ModuleHelper.GetTypesToImport(propertyTypes)
                                .Where(t => !genericArguments.Contains(t));

            return imports;
        }

        protected string GetHeader()
        {
            string name = this.Type.ToString(new ToTypeScriptOptions().WithNullable(false));
            return $"export interface {name} {{";
        }

        protected string GetFooter() => "}";

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
                        Predicate<Type> includeNamespacePredicate = type => this.Type.Namespace != type.Namespace;
                        var options = new ToTypeScriptOptions().WithIncludeNamespace(includeNamespacePredicate)
                                                               .WithNullable(this.StrictNullCheck && nullable);
                        return $"{propertyName}: { a.Property.PropertyType.ToTypeScript().ToString(options)};";
                    });

        public override IEnumerable<string> GetLines() =>
            new[] { this.GetHeader() }
            .Concat(this.GetBody())
            .Concat(this.GetFooter());
    }
}
