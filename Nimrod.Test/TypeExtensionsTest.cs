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
    public class TypeExtensionsTest
    {
        [Test]
        public void GetFileName()
        {
            var filename = typeof(GenericClass<int>).GetTypeScriptFilename();

            Assert.AreEqual("Nimrod.Test.ModelExamples.GenericClass.ts", filename);
        }
    }
}
