using Microsoft.Win32.SafeHandles;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private class Animal { }
        private class Duck : Animal { }
        private class MalardDuck : Duck { }

        [Test]
        public void GetBaseTypesTests()
        {
            var result = typeof(MalardDuck).GetBaseTypes().ToList();
            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(typeof(Duck), result[0]);
            Assert.AreEqual(typeof(Animal), result[1]);
            Assert.AreEqual(typeof(object), result[2]);
        }
    }
}
