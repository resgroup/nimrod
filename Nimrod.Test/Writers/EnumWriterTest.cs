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
    [TestFixture]
    public class EnumWriterTest
    {
        [Test]
        public void GetTypescriptType_ArrayLike_Test()
        {
            var writer = new EnumWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                writer.Write(stringWriter, typeof(Fruits));
            }
            string ts = builder.ToString();
            Assert.IsTrue(ts.Contains("enum IFruits"));
            Assert.IsTrue(ts.Contains("Apple = 0,"));
            Assert.IsTrue(ts.Contains("Orange = 5,"));
            Assert.IsTrue(ts.Contains("Ananas = 6,"));
        }
    }
}
