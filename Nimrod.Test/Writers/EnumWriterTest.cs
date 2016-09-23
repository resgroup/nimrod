using Nimrod.Writers.Default;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
    enum SomeEnumHexa : byte
    {
        SomeValue = 0x01,
    }
    [TestFixture]
    public class EnumWriterTest
    {
        [Test]
        public void GetTypescriptType_ArrayLike_Test()
        {
            var writer = new EnumToDefaultTypeScript(typeof(Fruits).ToTypeScript());
            var lines = writer.GetLines();
            string ts = lines.JoinNewLine();
            Assert.IsTrue(ts.Contains("enum Fruits"));
            Assert.IsTrue(ts.Contains("Apple = 0,"));
            Assert.IsTrue(ts.Contains("Orange = 5,"));
            Assert.IsTrue(ts.Contains("Ananas = 6,"));
        }

        [Test]
        [ExpectedException(typeof(NotSupportedException))]
        public void EnumTypesNotIntNotSupported()
        {
            var writer = new EnumToDefaultTypeScript(typeof(SomeEnumHexa).ToTypeScript());
            var value = writer.GetLines().ToList();
            Assert.Fail("Should not reach this point", value);
        }
    }
}
