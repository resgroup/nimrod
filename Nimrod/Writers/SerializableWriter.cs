using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    public class SerializableWriter : BaseWriter
    {
        public SerializableWriter(ModuleType module) : base(module)
        {
        }

        public override void Write(TextWriter writer, Type type)
        {
            var tsClassType = type.ToTypeScript();

            if (Module == ModuleType.TypeScript)
            {
                WriteLine(writer, string.Format("module {0} {1}", type.Namespace, '{'));
                IncrementIndent();
                WriteLine(writer, $"export class {tsClassType} extends String {"{}"}");
                DecrementIndent();
                WriteLine(writer, "}");
            }
            
            if (Module == ModuleType.Require)
            {
                WriteLine(writer, $"class {tsClassType} extends String {"{}"}");
                WriteLine(writer, $"export = {tsClassType};");
            }
        }
    }
}
