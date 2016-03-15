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
    public class ControllerWriterTest
    {

        [Test]
        public void Write_SimpleController()
        {
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                var writer = new ControllerWriter(stringWriter, ModuleType.TypeScript);
                writer.Write(typeof(MovieController));
            }
            string ts = builder.ToString();
            Assert.IsFalse(ts.Contains("Foo"));
        }
    }
}

