using Nimrod.Test.ModelExamples;
using Nimrod.Writers.Default;
using Nimrod.Writers.Require;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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

    public class DataMemberSpecificName
    {
        [DataMember(Name = "bar")]
        public float Foo { get; set; }
    }

    public class ModelWriterTests
    {
        [Test]
        public void WriteModel_UseDataMemberName()
        {
            var writer = new ModelToDefaultTypeScript(typeof(DataMemberSpecificName));
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            Assert.IsTrue(ts.Contains("bar"));
            Assert.IsFalse(ts.Contains("Foo"));
        }

        [Test]
        public void WriteModel_IgnoreDataMember()
        {
            var writer = new ModelToDefaultTypeScript(typeof(IgnoreDataMemberClass));
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            Assert.IsFalse(ts.Contains("Foo"));
        }

        [Test]
        public void GetTypescriptType_Generic()
        {
            var genericTypeDefinition = typeof(GenericFoo<int>).GetGenericTypeDefinition();
            var writer = new ModelToDefaultTypeScript(genericTypeDefinition);
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());

            Assert.IsTrue(ts.Contains("GenericFoo<T>"));
            Assert.IsTrue(ts.Contains("GenericProperty: T"));
            Assert.IsTrue(ts.Contains("GenericContainer: T[]"));
            Assert.IsTrue(ts.Contains("NonGenericProperty: number"));
        }
        [Test]
        public void GetTypescriptType_GenericListContainer()
        {
            var writer = new ModelToDefaultTypeScript(typeof(BarWrapper<int>).GetGenericTypeDefinition());
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            Assert.IsTrue(ts.Contains("Bars: T[];"));
        }

        [Test]
        public void GetTypescriptType_GenericCustomContainer()
        {
            var writer = new ModelToDefaultTypeScript(typeof(Fuzz<int>).GetGenericTypeDefinition());
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            Assert.IsTrue(ts.Contains("Fuzzs: Nimrod.Test.IGenericFoo<T>;"));
        }

        [Test]
        public void ModelWriter_RequireExportWithoutGenericArgument()
        {
            var writer = new ModelToRequireTypeScript(typeof(Fuzz<int>));
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            Assert.IsTrue(ts.Contains("export = IFuzz;"));
        }

        [Test]
        public void WriteModel_Movie()
        {
            var writer = new ModelToDefaultTypeScript(typeof(Movie));
            string ts = string.Join(Environment.NewLine, writer.GetLines().ToArray());
            //string ts = builder.ToString();
        }

    }
}

