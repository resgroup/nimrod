using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod.Test.Extensions
{
    [TestFixture]
    public class StringExtensionsTests
    {
        [Test]
        public void Repeat_Normal()
        {
            Assert.AreEqual("", "ab".Repeat(0));
            Assert.AreEqual("ababab", "ab".Repeat(3));
        }
        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Repeat_OutOfRange()
        {
            "ab".Repeat(-1);
        }

        [Test]
        public void HasNonEmbededWhiteSpace()
        {
            Assert.IsFalse("ab".HasNonEmbededWhiteSpace());
            Assert.IsTrue("a a".HasNonEmbededWhiteSpace());

            Assert.IsFalse("(a a)".HasNonEmbededWhiteSpace());

            Assert.IsFalse("(a (a a))".HasNonEmbededWhiteSpace());

            Assert.IsFalse("((a) a a))".HasNonEmbededWhiteSpace());
            Assert.IsTrue("(a (a a)) a".HasNonEmbededWhiteSpace());

            Assert.IsFalse("((string | null)[] | null)[]".HasNonEmbededWhiteSpace());
        }

    }
}
