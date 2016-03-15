using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class IndentingTextWriter
    {
        protected TextWriter _textWriter { get; }
        public string IndentString { get; }

        public int IndentLevel { get; private set; } = 0;

        public IndentingTextWriter(TextWriter textWriter, string indentString)
        {
            _textWriter = textWriter;
            IndentString = indentString;
        }

        public void WriteLine()
        {
            _textWriter.WriteLine();
        }

        public virtual void WriteLine(string line)
        {
            WriteIndent();
            _textWriter.WriteLine(line);
        }

        public void WriteIndent()
        {
            for (int i = 0; i < IndentLevel; i++)
            {
                _textWriter.Write(IndentString);
            }
        }

        public void Indent(int indent) => IndentLevel += indent;
        public void Outdent(int indent)
        {
            if (IndentLevel - indent < 0)
            {
                throw new InvalidOperationException("Cannot outdent, level is already at zero. There is a problem in the indentation problem.");
            }
            IndentLevel -= indent;
        }
    }
}
