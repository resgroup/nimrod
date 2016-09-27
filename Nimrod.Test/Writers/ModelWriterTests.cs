using Nimrod.Test.ModelExamples;
using Nimrod.Writers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

    public class ModelWriterTests
    {
        private class DataMemberSpecificName
        {
            [DataMember(Name = "bar")]
            public float Foo { get; set; }

            [DataMember(Name = "   ")]
            public float EmptyName { get; set; }
        }
        [Test]
        public void WriteModel_UseDataMemberName()
        {
            var writer = new ModelToTypeScript(typeof(DataMemberSpecificName).ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("bar"));
            Assert.IsFalse(ts.Contains("Foo"));
        }

        [Test]
        public void WriteModel_DoNotUseDataMemberName_IfEmptyOrWhitespace()
        {
            var writer = new ModelToTypeScript(typeof(DataMemberSpecificName).ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("EmptyName"));
        }

        [Test]
        public void WriteModel_IgnoreDataMember()
        {
            var writer = new ModelToTypeScript(typeof(IgnoreDataMemberClass).ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsFalse(ts.Contains("Foo"));
        }

        [Test]
        public void GetTypescriptType_Generic()
        {
            var genericTypeDefinition = typeof(GenericFoo<int>).GetGenericTypeDefinition();
            var writer = new ModelToTypeScript(genericTypeDefinition.ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();

            Assert.IsTrue(ts.Contains("interface GenericFoo<T> {"));
            Assert.IsTrue(ts.Contains("GenericProperty: T | null;"));
            Assert.IsTrue(ts.Contains("GenericContainer: (T | null)[] | null;"));
            Assert.IsTrue(ts.Contains("NonGenericProperty: number;"));
        }
        [Test]
        public void GetTypescriptType_GenericListContainer()
        {
            var writer = new ModelToTypeScript(typeof(BarWrapper<int>).GetGenericTypeDefinition().ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Bars: (T | null)[] | null;"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var writer = new ModelToTypeScript(typeof(Fuzz<int>).GetGenericTypeDefinition().ToTypeScript(), true, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("Fuzzs: GenericFoo<T> | null;"));
        }

        [Test]
        public void ModelWriter_RequireExportWithoutGenericArgument()
        {
            var writer = new ModelToTypeScript(typeof(Fuzz<int>).ToTypeScript(), false, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsTrue(ts.Contains("export default Fuzz;"));
        }
    }
}

