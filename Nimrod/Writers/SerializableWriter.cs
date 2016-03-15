using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    public class SerializableWriter : BaseWriter
    {
        public SerializableWriter(TextWriter writer, ModuleType moduleType) : base(writer, moduleType)
        {
        }

        public override void Write(Type type)
        {
            var tsClassType = type.ToTypeScript();

            if (Module == ModuleType.TypeScript)
            {
                WriteLine(string.Format("module {0} {1}", type.Namespace, '{'));
                WriteLine($"export class {tsClassType} extends String {"{}"}");
                WriteLine("}");
            }
            
            if (Module == ModuleType.Require)
            {
                WriteLine($"class {tsClassType} extends String {"{}"}");
                WriteLine($"export = {tsClassType};");
            }
        }
    }
}
