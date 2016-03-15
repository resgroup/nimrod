using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class AutoIndentingTextWriter : IndentingTextWriter
    {
        public AutoIndentingTextWriter(TextWriter textWriter, string indentString) : base(textWriter, indentString)
        {
        }

        public void Write(string partOfLine)
        {
            _textWriter.Write(partOfLine);
        }

        public override void WriteLine(string line)
        {
            if (line.Contains('}'))
            {
                Outdent();
            }
            base.WriteLine(line);
            if (line.Contains('{'))
            {
                Indent();
            }
        }
    }
}
