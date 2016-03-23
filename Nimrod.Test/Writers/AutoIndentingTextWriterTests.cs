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
            var lines = new[] {
                        "{",
                         "{}",
                        "hello{",
                          "}",
                           "}"
                 };
            string actual = string.Join(Environment.NewLine, lines.IndentLines("_"));
            var expected =
@"{
_{}
_hello{
_}
}";
            Assert.AreEqual(expected, actual);
        }
        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AutoIndentingTextWriter_TooMuchOutdent()
        {
                var lines = new[] {
                        "{",
                         "}",
                        "}"
                 };
            var indetedLines = lines.IndentLines().ToList();
        }
    }
}

