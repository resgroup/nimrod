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

        public void Write(string text)
        {
            ComputeIndentAndWrite(text);
            _textWriter.Write(text);
        }

        public override void WriteLine(string text)
        {
            ComputeIndentAndWrite(text, t => base.WriteLine(t));
        }

        private void ComputeIndentAndWrite(string text, Action<string> action = null)
        {
            var opened = text.Count(c => c == '{');
            var closed = text.Count(c => c == '}');
            var indentation = opened - closed;

            if (indentation < 0)
            {
                Outdent(-indentation);
            }
            // outdentation has to be done before writing
            if (action != null)
            {
                action(text);
            }
            // whereas indentation has to be done after writing
            if (indentation > 0)
            {
                Indent(indentation);
            }
        }
    }
}
