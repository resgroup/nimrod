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
    public class ControllerWriterTests
    {

        [Test]
        public void WriteStandard()
        {
            var writer = new ControllerWriter(ModuleType.TypeScript);
            var builder = new StringBuilder();
            using (var stringWriter = new StringWriter(builder))
            {
                writer.Write(stringWriter, typeof(MovieController));
            }
            //string ts = builder.ToString();
        }
        

    }
}

