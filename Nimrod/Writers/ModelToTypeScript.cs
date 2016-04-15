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

        public ModelToTypeScript(Type type) : base(type)
        {
        }

        protected abstract IEnumerable<string> GetHeader();
        protected abstract IEnumerable<string> GetFooter();

        public override IEnumerable<string> GetLines()
        {
            foreach (var line in this.GetHeader())
            {
                yield return line;
            }
            foreach (var property in this.Type.GetProperties())
            {
                var attributes = Attribute.GetCustomAttributes(property);
                var ignoreDataMemberAttributes = attributes.OfType<IgnoreDataMemberAttribute>();
                if (ignoreDataMemberAttributes.IsEmpty())
                {
                    var dataMemberAttribute = attributes.OfType<DataMemberAttribute>().FirstOrDefault();
                    string propertyName;
                    if (dataMemberAttribute != null && !string.IsNullOrWhiteSpace(dataMemberAttribute.Name))
                    {
                        // the Name of the DataMember attribute is going to be used by serializer
                        // this is the value we need to write into the file
                        propertyName = dataMemberAttribute.Name;
                    }
                    else
                    {
                        propertyName = property.Name;
                    }

                    var tsPropertyType = property.PropertyType.ToTypeScript(PrefixPropertyWithNamespace);
                    yield return $"{propertyName}: {tsPropertyType};";
                }
            }

            foreach (var line in this.GetFooter())
            {
                yield return line;
            }
        }
    }
}
