using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test
{
    public class NonGenericClass { }

    [TestFixture]
    public class ToTypescriptTests
    {
        [Test]
        public void IsTupleTest()
        {
            Assert.IsTrue(typeof(Tuple<int>).IsTuple());
            Assert.IsTrue(typeof(Tuple<int, int>).IsTuple());
            Assert.IsTrue(typeof(Tuple<int, int, Tuple<int, int>>).IsTuple());
            Assert.IsTrue(typeof(Tuple<>).IsTuple());
            Assert.IsTrue(typeof(Tuple<,>).IsTuple());
            Assert.IsFalse(typeof(int).IsTuple());
            Assert.IsFalse(new object().GetType().IsTuple());
        }

        [Test]
        public void GetTypescriptType_ArrayLike_Test()
        {
            var expected = "(string | null)[] | null";
            Assert.AreEqual(expected, typeof(List<string>).ToTypeScript().ToString());
            Assert.AreEqual(expected, typeof(IEnumerable<string>).ToTypeScript().ToString());
            Assert.AreEqual(expected, typeof(ICollection<string>).ToTypeScript().ToString());
            Assert.AreEqual(expected, typeof(string[]).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_ArrayofArrays_Test()
        {
            var expected = "((string | null)[] | null)[] | null";
            Assert.AreEqual(expected, typeof(List<List<string>>).ToTypeScript().ToString());
            Assert.AreEqual(expected, typeof(List<string[]>).ToTypeScript().ToString());
            Assert.AreEqual(expected, typeof(List<string>[]).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_Dictionary_Test()
        {
            Assert.AreEqual("{ [id: number] : number; } | null", typeof(Dictionary<int, int>).ToTypeScript().ToString());
            Assert.AreEqual("{ [id: number] : (string | null)[] | null; } | null", typeof(Dictionary<int, List<string>>).ToTypeScript().ToString());
            Assert.AreEqual("{ [id: number] : (NonGenericClass | null)[] | null; } | null", typeof(Dictionary<int, List<NonGenericClass>>).ToTypeScript().ToString());
        }

        public class Generic<T>
        {
            public T FooValue { get; set; }
        }
        public class Generic1<T>
        {
            public T FooValue { get; set; }
        }

        [Test]
        public void GetTypescriptType_Generic_Test()
        {
            Assert.AreEqual("Nimrod.Test.Generic<string> | null", typeof(Generic<string>).ToTypeScript().ToString(true, true));
            Assert.AreEqual("Nimrod.Test.Generic | null", typeof(Generic<string>).ToTypeScript().ToString(true, false));
            Assert.AreEqual("Generic<string> | null", typeof(Generic<string>).ToTypeScript().ToString(false, true));
            Assert.AreEqual("Generic | null", typeof(Generic<string>).ToTypeScript().ToString(false, false));
        }

        [Test]
        public void GetTypescriptType_GenericWithNumber_Test()
        {
            Assert.AreEqual("Nimrod.Test.Generic1<string> | null", typeof(Generic1<string>).ToTypeScript().ToString(true, true));
        }

        [Test]
        public void GetTypescriptType_NonGeneric_Test()
        {
            var actual = typeof(NonGenericClass).ToTypeScript().ToString(true);
            Assert.AreEqual("Nimrod.Test.NonGenericClass | null", actual);
        }

        [Test]

        public void GetTypescriptType_BaseNumberType_Test()
        {
            Assert.AreEqual("number", typeof(short).ToTypeScript().ToString());
            Assert.AreEqual("number", typeof(int).ToTypeScript().ToString());
            Assert.AreEqual("number", typeof(long).ToTypeScript().ToString());
            Assert.AreEqual("number", typeof(float).ToTypeScript().ToString());
            Assert.AreEqual("number", typeof(double).ToTypeScript().ToString());
            Assert.AreEqual("number", typeof(decimal).ToTypeScript().ToString());

        }

        [Test]
        public void GetTypescriptType_Nullable_Test()
        {
            Assert.AreEqual("number | null", typeof(short?).ToTypeScript().ToString());
            Assert.AreEqual("number | null", typeof(int?).ToTypeScript().ToString());
            Assert.AreEqual("number | null", typeof(long?).ToTypeScript().ToString());
            Assert.AreEqual("number | null", typeof(float?).ToTypeScript().ToString());
            Assert.AreEqual("number | null", typeof(double?).ToTypeScript().ToString());
            Assert.AreEqual("number | null", typeof(decimal?).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_Enum_Test()
        {
            Assert.AreEqual("FooCountToThree | null", typeof(FooCountToThree?).ToTypeScript().ToString(new ToTypeScriptOptions().WithNullable(true)));
            Assert.AreEqual("FooCountToThree", typeof(FooCountToThree).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_Date_Test()
        {
            Assert.AreEqual("Date", typeof(DateTime).ToTypeScript().ToString());
            Assert.AreEqual("Date", typeof(DateTimeOffset).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_Object_Test()
        {
            Assert.AreEqual("any | null", typeof(System.Object).ToTypeScript().ToString());
            Assert.AreEqual("any | null", typeof(object).ToTypeScript().ToString());
        }

        [Test]
        public void GetTypescriptType_GenericPropertyOfGenericClass()
        {
            var typeDefinition = typeof(GenericClass<int>).GetGenericTypeDefinition();

            var result = typeDefinition.GetProperties()
                                       .Select(property => property.PropertyType.ToTypeScript())
                                       .ToList();
            Assert.IsTrue(result.All(s => s.ToString() == "(TFoo | null)[] | null"));
        }

    }

    public enum FooCountToThree
    {
        One, Two, Three
    }
}
