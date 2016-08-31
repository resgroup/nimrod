using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Nimrod
{
    public abstract class ModelToTypeScript : ToTypeScript
    {
        public virtual bool PrefixPropertyWithNamespace => false;
        public string TsName => this.Type.ToTypeScript();

        public ModelToTypeScript(Type type) : base(type) { }

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();

        private IEnumerable<string> GetBody() => this.Type.GetProperties()
                    .Select(property => new
                    {
                        Attributes = Attribute.GetCustomAttributes(property),
                        Property = property,
                        TypeScriptProperty = property.PropertyType.ToTypeScript(PrefixPropertyWithNamespace)
                    })
                    // do not write attribute that are not serialize throught the [IgnoreDataMember] attribute
                    .Where(a => a.Attributes.OfType<IgnoreDataMemberAttribute>().IsEmpty())
                    .Select(a =>
                    {
                        var attributeName = a.Attributes.OfType<DataMemberAttribute>().FirstOrDefault()?.Name;
                        string propertyName = string.IsNullOrWhiteSpace(attributeName) ? a.Property.Name : attributeName;
                        return $"{propertyName}: {a.TypeScriptProperty};";
                    });

        public override IEnumerable<string> GetLines() =>
            this.GetHeader()
            .Concat(this.GetBody())
            .Concat(this.GetFooter());
    }
}
