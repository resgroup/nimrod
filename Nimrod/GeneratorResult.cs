using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using Nimrod.Writers;
using System.Linq;

namespace Nimrod
{
    public class GeneratorResult
    {
        private List<ToTypeScript> ToTypeScripts;

        public IEnumerable<ToTypeScript> Services => this.ToTypeScripts.Where(t => t.ObjectType == ObjectType.Controller);
        public IEnumerable<ToTypeScript> Models => this.ToTypeScripts.Where(t => t.ObjectType != ObjectType.Controller);
        public List<FileToWrite> Files { get; }

        public GeneratorResult(List<ToTypeScript> toTypeScripts, List<FileToWrite> files)
        {
            this.ToTypeScripts = toTypeScripts.ThrowIfNull(nameof(toTypeScripts));
            this.Files = files.ThrowIfNull(nameof(files));
        }
    }
}