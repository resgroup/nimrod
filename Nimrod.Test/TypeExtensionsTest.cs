using Microsoft.Win32.SafeHandles;
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
        public void IsSystem()
        {
            Assert.IsTrue(typeof(string).IsSystem());
            Assert.IsTrue(typeof(int).IsSystem());
            Assert.IsTrue(typeof(object).IsSystem());
            Assert.IsTrue(typeof(SafeWaitHandle).IsSystem());
        }
    }
}
