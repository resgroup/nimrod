using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Very.Specific.NameSpace;

namespace Very.Specific.NameSpace
{
    public class SomeClass
    {
        public int SomeProperty { get; set; }
    }

    public class SomeGenericClass<T>
    {
        public T SomeGenericProperty { get; set; }
    }
}

namespace Nimrod.Test
{
    [TestFixture]
    public class TupleToTypeScriptTests
    {
        [Test]
        public void TupleToTypeScriptTests_NameSpaceInclude_Test()
        {
            Assert.AreEqual("{ Item1: ISomeClass }", typeof(Tuple<SomeClass>).ToTypeScript(false));
            Assert.AreEqual("{ Item1: Very.Specific.NameSpace.ISomeClass }", typeof(Tuple<SomeClass>).ToTypeScript(true));
        }

        [Test]
        public void TupleToTypeScriptTests_Generic_Test()
        {
            Assert.AreEqual("{ Item1: ISomeGenericClass<number> }", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript(false, true));
            Assert.AreEqual("{ Item1: Very.Specific.NameSpace.ISomeGenericClass<number> }", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript(true, true));

            Assert.AreEqual("{ Item1: ISomeGenericClass }", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript(false, false));
            Assert.AreEqual("{ Item1: Very.Specific.NameSpace.ISomeGenericClass }", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript(true, false));
        }

        [Test]
        public void TupleToTypeScriptTests_BaseObject_Test()
        {
            Assert.AreEqual("{ Item1: number }", typeof(Tuple<int>).ToTypeScript());
            Assert.AreEqual("{ Item1: string }", typeof(Tuple<string>).ToTypeScript());
            Assert.AreEqual("{ Item1: number, Item2: string }", typeof(Tuple<int, string>).ToTypeScript());
            Assert.AreEqual("{ Item1: number, Item2: string, Item3: Date }", typeof(Tuple<int, string, DateTime>).ToTypeScript());
            Assert.AreEqual("{ Item1: { Item1: number } }", typeof(Tuple<Tuple<int>>).ToTypeScript());
        }

    }
}
