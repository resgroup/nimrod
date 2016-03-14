using System.Collections.Generic;

namespace Nimrod
{
    public interface IOptions
    {
        IList<string> Files { get; set; }
        bool Help { get; set; }
        string Module { get; set; }
        ModuleType ModuleType { get; }
        string OutputPath { get; set; }
        bool Verbose { get; set; }

        void Write(string value);
        void WriteLine(string value);
    }
}