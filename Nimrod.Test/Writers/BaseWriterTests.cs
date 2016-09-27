using Nimrod.Writers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    [TestFixture]
    public class BaseWriterTests
    {
        public class GenericItem<T>
        {
            public T Item { get; set; }
        }
        public class GenericWrapper<T>
        {
            public GenericItem<T> Item { get; set; }
        }

        [Test]
        public void WriteModel_WriteImports_GenericWrapper()
        {
            var lines = ModuleHelper.GetImportLine(typeof(GenericWrapper<>));
            Assert.AreEqual("import IGenericWrapper from './Nimrod.Test.GenericWrapper';", lines);
        }

        [Test]
        public void WriteModel_WriteImports_GenericItem()
        {
            var lines = ModuleHelper.GetImportLine(typeof(GenericItem<>));
            Assert.AreEqual("import IGenericItem from './Nimrod.Test.GenericItem';", lines);
        }

        [Test]
        public void GetTypescriptType_GenericListContainer()
        {
            var genericTypeDefinition = typeof(BarWrapper<int>).GetGenericTypeDefinition().ToTypeScript();
            var writer = new ModelToTypeScript(genericTypeDefinition, true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Bars: (T | null)[] | null;"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var genericTypeDefinition = typeof(Fuzz<int>).GetGenericTypeDefinition().ToTypeScript();
            var writer = new ModelToTypeScript(genericTypeDefinition, true, true);

            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Fuzzs: Nimrod.Test.IGenericFoo<T> | null;"));
        }

    }
}

