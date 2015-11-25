using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Nimrod
{
    /// <summary>
    /// Base class to write typescript file
    /// </summary>
    public abstract class BaseWriter
    {

        private int Indent { get; set; }

        public ModuleType Module { get; }

        public BaseWriter(ModuleType module)
        {
            Module = module;
            Indent = 0;
        }

        public abstract void Write(TextWriter writer, Type type);

        protected void IncrementIndent()
        {
            Indent++;
        }

        protected void DecrementIndent()
        {
            Indent--;
        }

        protected void WriteIndent(TextWriter writer)
        {
            for (int i = 0; i < Indent; i++)
            {
                Write(writer, "    ");   // 4 spaces
            }
        }

        protected void Write(TextWriter writer, string text)
        {
            writer.Write(text);
        }

        protected void WriteLine(TextWriter writer, string line)
        {
            WriteIndent(writer);
            writer.WriteLine(line);
        }

        protected void WriteLine(TextWriter writer)
        {
            writer.WriteLine();
        }


        public static void WriteImports(TextWriter writer, IEnumerable<Type> importedTypes, IEnumerable<Type> exclude = null)
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
                    writer.WriteLine($"import {typeName} = require('./{moduleName}');");
                }
            }
        }
    }
}
