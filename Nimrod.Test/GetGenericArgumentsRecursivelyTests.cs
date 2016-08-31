using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    [TestFixture]
    public class GetGenericArgumentsRecursivelyTests
    {
        [Test]
        public void StandardTest()
        {
            var type = typeof(List<IEnumerable<Dictionary<int, int>>>);
            var args = type.GetGenericArgumentsRecursively().OrderBy(t => t.Name).ToList();

            Assert.AreEqual(3, args.Count);
            Assert.AreEqual(args[0], typeof(Dictionary<int, int>));
            Assert.AreEqual(args[1], typeof(IEnumerable<Dictionary<int, int>>));
            Assert.AreEqual(args[2], typeof(int));
        }

        [Test]
        public void ListsContainingListsTest_DeepSearch()
        {
            var type = typeof(List<List<List<List<List<int>>>>>);
            var args = type.GetGenericArgumentsRecursively().OrderBy(t => t.Name).ToList();

            Assert.AreEqual(5, args.Count);
            Assert.AreEqual(args[0], typeof(int));
            Assert.AreEqual(args[1], typeof(List<int>));
            Assert.AreEqual(args[2], typeof(List<List<int>>));
            Assert.AreEqual(args[3], typeof(List<List<List<int>>>));
            Assert.AreEqual(args[4], typeof(List<List<List<List<int>>>>));
        }

        [Test]
        public void GenericsContainingMultipleGenerics_TransversalSearch()
        {
            var type = typeof(Tuple<int, string, double>);
            var args = type.GetGenericArgumentsRecursively().OrderBy(t => t.Name).ToList();

            Assert.AreEqual(3, args.Count);
            Assert.AreEqual(args[0], typeof(double));
            Assert.AreEqual(args[1], typeof(int));
            Assert.AreEqual(args[2], typeof(string));
        }
    }

}
