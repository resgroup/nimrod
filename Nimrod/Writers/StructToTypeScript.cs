using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod.Writers
{
    /// <summary>
    /// For classes which extends String, ie: which are Serializable
    /// </summary>
    public class StructToTypeScript : ToTypeScript
    {
        public override FileType FileType => FileType.Struct;

        public StructToTypeScript(TypeScriptType type, bool strictNullCheck, bool singleFile)
            : base(type, strictNullCheck, singleFile)
        {
            if (!this.Type.Type.IsValueType)
            {
                throw new ArgumentException($"{this.Type.Name} is not a Struct.", nameof(type));
            }
        }

        public override IEnumerable<string> GetImports() => new List<string>();

        public override IEnumerable<string> GetLines() => new[] {
            this.SingleFile ? $"export default class {this.Type} extends String {{}}" : $"export class {this.Type} extends String {{}}"
        };
    }
}
