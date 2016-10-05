using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Nimrod
{
    public class GeneratorResult
    {
        public List<string> Services { get; }
        public List<string> Models { get; }
        public List<FileToWrite> Files { get; }

        public GeneratorResult(List<string> services, List<string> models, List<FileToWrite> files)
        {
            this.Services = services.ThrowIfNull(nameof(services));
            this.Models = models.ThrowIfNull(nameof(models));
            this.Files = files.ThrowIfNull(nameof(files));
        }

        public override string ToString()
        {
            using (var stream = new MemoryStream())
            {
                var serializer = new DataContractJsonSerializer(typeof(GeneratorResult));
                serializer.WriteObject(stream, this);
                stream.Position = 0;
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}