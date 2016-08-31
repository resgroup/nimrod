using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod.Test
{
    [TestFixture]
    public class GeneratorTests
    {
        [Test]
        public void GetFileNameTest()
        {
            var filename = Generator.GetTypeScriptFilename(typeof(GenericClass<int>));

            Assert.AreEqual("Nimrod.Test.ModelExamples.GenericClass.ts", filename);
        }

    }
}
