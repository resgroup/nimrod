using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    [TestFixture]
    public class ToTReferencedTypesTests
    {
        [Test]
        public void StandardTest()
        {
            var type = typeof(List<IEnumerable<Dictionary<int, int>>>);
            var args = type.ReferencedTypes().OrderBy(t => t.Name).ToList();

            Assert.AreEqual(3, args.Count);
            Assert.AreEqual(args[0], typeof(Dictionary<int, int>).GetGenericTypeDefinition());
            Assert.AreEqual(args[1], typeof(IEnumerable<int>).GetGenericTypeDefinition());
            Assert.AreEqual(args[2], typeof(int));
        }
    }

}
