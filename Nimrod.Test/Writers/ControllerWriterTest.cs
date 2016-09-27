using Nimrod.Test.ModelExamples;
using Nimrod.Writers;
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
            var writer = new ControllerToTypeScript(typeof(MovieController).ToTypeScript(), false, true);
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsFalse(ts.Contains("Foo"));
        }
    }
}

