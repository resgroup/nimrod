using Nimrod.Test.ModelExamples;
using Nimrod.Writers.Default;
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
            string ts = writer.GetLines().JoinNewLine();
            Assert.IsFalse(ts.Contains("Foo"));
        }
    }
}

