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
    public class AutoIndentingTextWriterTests
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
        public void AutoIndentingTextWriter_StandardCase()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new AutoIndentingTextWriter(stringWriter, "_");
                writer.WriteLine("{");
                writer.WriteLine("{}");
                writer.Write("hello");
                writer.WriteLine("{");
                writer.WriteLine("}");
                writer.WriteLine("}");
            }
            string actual = builder.ToString();
            var expected =
@"{
_{}
hello_{
_}
}
";
            Assert.AreEqual(expected, actual);
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoIndentingTextWriter_TooMuchOutdent()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new AutoIndentingTextWriter(stringWriter, "_");
                writer.WriteLine("{");
                writer.WriteLine("}");
                // illegal should happen here
                writer.WriteLine("}");
            }
        }
    }
}

