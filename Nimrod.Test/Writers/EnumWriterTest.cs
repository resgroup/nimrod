using Nimrod.Writers.Default;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    public enum Fruits
    {
        Apple,
        Orange = 5,
        Ananas
    }
    enum SomeEnum : byte
    {
        SomeValue = 0x01,
    }
    [TestFixture]
    public class EnumWriterTest
    {
        [Test]
        public void GetTypescriptType_ArrayLike_Test()
        {
            var writer = new EnumToDefaultTypeScript(typeof(Fruits));
            var lines = writer.GetLines();
            string ts = string.Join(Environment.NewLine, lines);
            Assert.IsTrue(ts.Contains("enum IFruits"));
            Assert.IsTrue(ts.Contains("Apple = 0,"));
            Assert.IsTrue(ts.Contains("Orange = 5,"));
            Assert.IsTrue(ts.Contains("Ananas = 6,"));
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void EnumTypesNotIntNotSupported()
        {
            var writer = new EnumToDefaultTypeScript(typeof(SomeEnum));
            var lines = writer.GetLines().ToList();
        }
    }
}
