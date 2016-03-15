using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class IndentingTextWriter
    {
        protected TextWriter _textWriter { get; }
        public string IndentString { get; }

        private int _indentLevel;

        public IndentingTextWriter(TextWriter textWriter, string indentString)
        {
            _textWriter = textWriter;
            _indentLevel = 0;
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
            for (int i = 0; i < _indentLevel; i++)
            {
                _textWriter.Write(IndentString);
            }
        }

        public TidyUp AutoCloseIndent()
        {
            Indent();
            return new TidyUp(() => Outdent());
        }

        public void Indent() => _indentLevel++;
        public void Outdent() => _indentLevel--;
    }
}
