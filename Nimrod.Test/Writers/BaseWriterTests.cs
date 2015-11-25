using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    [TestFixture]
    public class BaseWriterTests
    {
        public class GenericItem<T> { }
        public class GenericWrapper<T>
        {
            public GenericItem<T> Item
            {
                get; set;
            }
        }

        [Test]
        public void WriteModel_WriteImports()
        {
            var writer = new ModelWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                BaseWriter.WriteImports(stringWriter, new[] { typeof(GenericWrapper<GenericItem<int>>) });
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("import IGenericItem = require('./Nimrod.Test.GenericItem');"));
            Assert.IsTrue(ts.Contains("import IGenericWrapper = require('./Nimrod.Test.GenericWrapper');"));
        }

        [Test]
        public void GetTypescriptType_Generic()
        {
            var writer = new ModelWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                writer.Write(stringWriter, typeof(GenericFoo<int>).GetGenericTypeDefinition());
            }
            string ts = builder.ToString();

            Assert.IsTrue(ts.Contains("GenericFoo<T>"));
            Assert.IsTrue(ts.Contains("GenericProperty: T"));
            Assert.IsTrue(ts.Contains("GenericContainer: T[]"));
            Assert.IsTrue(ts.Contains("NonGenericProperty: number"));
        }
        [Test]
        public void GetTypescriptType_GenericListContainer()
        {
            var writer = new ModelWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                writer.Write(stringWriter, typeof(BarWrapper<int>).GetGenericTypeDefinition());
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("Bars: T[];"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var writer = new ModelWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                writer.Write(stringWriter, typeof(Fuzz<int>).GetGenericTypeDefinition());
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("Fuzzs: Nimrod.Test.IGenericFoo<T>;"));
        }
        
    }
}

