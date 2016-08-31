using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nimrod.Test
{
    enum Fruit { Banana, Apple }
    [TestFixture]
    public class ObjectExtensionsTests
    {

        [Test]
        public void FirstOrDefaultHttpMethodAttribute_NoHttp()
        {
            var result = ObjectExtensions.GetEnumValues<Fruit>().OrderBy(t => t.ToString()).ToList();
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(Fruit.Apple, result[0]);
            Assert.AreEqual(Fruit.Banana, result[1]);
        }

    }
}
