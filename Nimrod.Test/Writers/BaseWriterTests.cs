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
            var imports = RequireModuleWriter.GetImports(new[] { typeof(GenericWrapper<GenericItem<int>>) })
                .OrderBy(s => s)
                .ToList();

            Assert.AreEqual(2, imports.Count);
            Assert.AreEqual("import IGenericItem = require('./Nimrod.Test.GenericItem');", imports[0]);
            Assert.AreEqual("import IGenericWrapper = require('./Nimrod.Test.GenericWrapper');", imports[1]);
        }

        [Test]
        public void GetTypescriptType_Generic()
        {
            var genericTypeDefinition = typeof(GenericFoo<int>).GetGenericTypeDefinition();
            var writer = new ModelToDefaultTypeScript(genericTypeDefinition);

            string ts = string.Join(Environment.NewLine, writer.Build().ToArray());

            Assert.IsTrue(ts.Contains("GenericFoo<T>"));
            Assert.IsTrue(ts.Contains("GenericProperty: T"));
            Assert.IsTrue(ts.Contains("GenericContainer: T[]"));
            Assert.IsTrue(ts.Contains("NonGenericProperty: number"));
        }
        [Test]
        public void GetTypescriptType_GenericListContainer()
        {
            var genericTypeDefinition = typeof(BarWrapper<int>).GetGenericTypeDefinition();
            var writer = new ModelToDefaultTypeScript(genericTypeDefinition);
            string ts = string.Join(Environment.NewLine, writer.Build().ToArray());
            Assert.IsTrue(ts.Contains("Bars: T[];"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var genericTypeDefinition = typeof(Fuzz<int>).GetGenericTypeDefinition();
            var writer = new ModelToDefaultTypeScript(genericTypeDefinition);

            string ts = string.Join(Environment.NewLine, writer.Build().ToArray());
            Assert.IsTrue(ts.Contains("Fuzzs: Nimrod.Test.IGenericFoo<T>;"));
        }

    }
}

