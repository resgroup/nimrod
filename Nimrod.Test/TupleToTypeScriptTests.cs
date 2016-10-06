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
            Assert.AreEqual("{ Item1: SomeClass | null } | null", typeof(Tuple<SomeClass>).ToTypeScript().ToString(type => false));
            Assert.AreEqual("{ Item1: Very_Specific_NameSpace.SomeClass | null } | null", typeof(Tuple<SomeClass>).ToTypeScript().ToString(type => true));
        }

        [Test]
        public void TupleToTypeScriptTests_Generic_Test()
        {
            Assert.AreEqual("{ Item1: SomeGenericClass<number> | null } | null", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript().ToString(type => false, true));
            Assert.AreEqual("{ Item1: Very_Specific_NameSpace.SomeGenericClass<number> | null } | null", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript().ToString(type => true, true));

            Assert.AreEqual("{ Item1: SomeGenericClass | null } | null", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript().ToString(type => false, false));
            Assert.AreEqual("{ Item1: Very_Specific_NameSpace.SomeGenericClass | null } | null", typeof(Tuple<SomeGenericClass<int>>).ToTypeScript().ToString(type => true, false));
        }

        [Test]
        public void TupleToTypeScriptTests_BaseObject_Test()
        {
            Assert.AreEqual("{ Item1: number } | null", typeof(Tuple<int>).ToTypeScript().ToString());
            Assert.AreEqual("{ Item1: string | null } | null", typeof(Tuple<string>).ToTypeScript().ToString());
            Assert.AreEqual("{ Item1: number, Item2: string | null } | null", typeof(Tuple<int, string>).ToTypeScript().ToString());
            Assert.AreEqual("{ Item1: number, Item2: string | null, Item3: Date } | null", typeof(Tuple<int, string, DateTime>).ToTypeScript().ToString());
            Assert.AreEqual("{ Item1: { Item1: number } | null } | null", typeof(Tuple<Tuple<int>>).ToTypeScript().ToString());
        }

    }
}
