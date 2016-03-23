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

        public override IEnumerable<string> Build()
        {
            foreach(var line in this.GetHeader())
            {
                yield return line;
            }
            foreach (var property in this.Type.GetProperties())
            {
                var attributes = Attribute.GetCustomAttributes(property);
                var ignoreDataMemberAttributes = attributes.Where(attribute => attribute.GetType() == typeof(IgnoreDataMemberAttribute));
                if (ignoreDataMemberAttributes.Any() == false)
                {
                    var tsPropertyType = property.PropertyType.ToTypeScript(PrefixPropertyWithNamespace);
                    yield return $"{property.Name}: {tsPropertyType};";
                }
            }

            foreach (var line in this.GetFooter())
            {
                yield return line;
            }
        }
    }
}
