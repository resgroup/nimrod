using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    public class GenericFoo<T>
    {
        public T GenericProperty { get; set; }
        public int NonGenericProperty { get; set; }
        public List<T> GenericContainer { get; set; }
    }

    public class BarWrapper<T>
    {
        public List<T> Bars { get; set; }
    }



    public class Fuzz<T>
    {
        public GenericFoo<T> Fuzzs { get; set; }
    }


    [TestFixture]
    public class ModelWriterTests
    {

        [Test]
        public void WriteModel_IgnoreDataMember()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(IgnoreDataMemberClass));
            }
            string ts = builder.ToString();
            Assert.IsFalse(ts.Contains("Foo"));
        }

        [Test]
        public void GetTypescriptType_Generic()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(GenericFoo<int>).GetGenericTypeDefinition());
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
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(BarWrapper<int>).GetGenericTypeDefinition());
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("Bars: T[];"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(Fuzz<int>).GetGenericTypeDefinition());
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("Fuzzs: Nimrod.Test.IGenericFoo<T>;"));
        }

        [Test]
        public void ModelWriter_RequireExportWithoutGenericArgument()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.Require);
                writer.Write(typeof(Fuzz<int>));
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("export = IFuzz;"));
        }

        [Test]
        public void WriteModel_Movie()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ModelWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(Movie));
            }
            //string ts = builder.ToString();
            
        }

    }
}

