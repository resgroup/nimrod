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
        public void GetTypescriptType_GenericListContainer()
        {
            var genericTypeDefinition = typeof(BarWrapper<int>).GetGenericTypeDefinition().ToTypeScript();
            var writer = new ModelToTypeScript(genericTypeDefinition, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Bars: (T | null)[] | null;"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var genericTypeDefinition = typeof(Fuzz<int>).GetGenericTypeDefinition().ToTypeScript();
            var writer = new ModelToTypeScript(genericTypeDefinition, true);

            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Fuzzs: GenericFoo<T> | null;"));
        }

    }
}

