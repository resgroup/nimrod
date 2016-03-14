using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics.Contracts;

namespace Nimrod
{
    /// <summary>
    /// Base class to write typescript file
    /// </summary>
    public class BaseWriter
    {

        protected AutoIndentingTextWriter Writer { get; }

        private int Indent { get; set; }
        private bool AtStartOfLine { get; set; }

        public ModuleType Module { get; }

        public BaseWriter(TextWriter writer, ModuleType module)
        {
            Contract.Requires(writer != null);
            
            Writer = new AutoIndentingTextWriter(writer, "    ");
            Module = module;
            Indent = 0;
            AtStartOfLine = true;
        }

        public virtual void Write(Type type) { }

        protected void Write(string text)
        {
            if (AtStartOfLine)
            {
                Writer.WriteIndent();
                AtStartOfLine = false;
            }

            Writer.Write(text);
        }

        protected void WriteLine(string line)
        {
            Writer.WriteLine(line);
            AtStartOfLine = true;
        }

        protected void WriteLine()
        {
            Writer.WriteLine();
            AtStartOfLine = true;
        }


        public void WriteImports(IEnumerable<Type> importedTypes, IEnumerable<Type> exclude = null)
        {
            var distinctByTemplates = importedTypes
                .SelectMany(type => type.ReferencedTypes().Concat(new[] { type.IsGenericType ? type.GetGenericTypeDefinition() : type }))
                .Where(type => !type.IsSystem())
                .Distinct();

            foreach (var type in distinctByTemplates)
            {
                if (exclude == null || exclude.Contains(type) == false)
                {
                    var typeName = type.ToTypeScript(false, false);
                    var moduleName = type.TypeScriptModuleName();
                    Writer.WriteLine($"import {typeName} = require('./{moduleName}');");
                }
            }
        }
    }
}
