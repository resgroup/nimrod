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
            var writer = new ControllerToDefaultTypeScript(typeof(MovieController));
            string ts = string.Join(Environment.NewLine, writer.Build().ToArray());
            Assert.IsFalse(ts.Contains("Foo"));
        }
    }
}

