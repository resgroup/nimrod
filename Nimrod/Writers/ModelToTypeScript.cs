using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod
{
    public abstract class ModelToTypeScript : ToTypeScript
    {
        public virtual bool PrefixPropertyWithNamespace => false;
        public string TsName => this.Type.ToString(new ToTypeScriptOptions().WithNullable(false));

        public ModelToTypeScript(TypeScriptType type, bool strictNullCheck) : base(type, strictNullCheck) { }

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();

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
            this.GetHeader()
            .Concat(this.GetBody())
            .Concat(this.GetFooter());
    }
}
