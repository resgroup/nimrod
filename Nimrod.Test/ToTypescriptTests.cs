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
            Assert.AreEqual("string[]", typeof(List<string>).ToTypeScript());
            Assert.AreEqual("string[]", typeof(IEnumerable<string>).ToTypeScript());
            Assert.AreEqual("string[]", typeof(ICollection<string>).ToTypeScript());
            Assert.AreEqual("string[]", typeof(string[]).ToTypeScript(false));
        }

        [Test]
        public void GetTypescriptType_ArrayofArrays_Test()
        {
            Assert.AreEqual("string[][]", typeof(List<List<string>>).ToTypeScript());
            Assert.AreEqual("string[][]", typeof(List<string[]>).ToTypeScript());
            Assert.AreEqual("string[][]", typeof(List<string>[]).ToTypeScript());
        }

        [Test]
        public void GetTypescriptType_Dictionary_Test()
        {
            Assert.AreEqual("{ [id: number] : number; }", typeof(Dictionary<int, int>).ToTypeScript());
            Assert.AreEqual("{ [id: number] : string[]; }", typeof(Dictionary<int, List<string>>).ToTypeScript());
            Assert.AreEqual("{ [id: string] : INonGenericClass[]; }", typeof(Dictionary<string, List<NonGenericClass>>).ToTypeScript());
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
            Assert.AreEqual("Nimrod.Test.IGeneric<string>", typeof(Generic<string>).ToTypeScript(true, true));
            Assert.AreEqual("Nimrod.Test.IGeneric", typeof(Generic<string>).ToTypeScript(true, false));
            Assert.AreEqual("IGeneric<string>", typeof(Generic<string>).ToTypeScript(false, true));
            Assert.AreEqual("IGeneric", typeof(Generic<string>).ToTypeScript(false, false));
        }

        [Test]
        public void GetTypescriptType_GenericWithNumber_Test()
        {
            Assert.AreEqual("Nimrod.Test.IGeneric1<string>", typeof(Generic1<string>).ToTypeScript(true, true));
        }

        [Test]
        public void GetTypescriptType_NonGeneric_Test()
        {
            var actual = typeof(NonGenericClass).ToTypeScript(true);
            Assert.AreEqual("Nimrod.Test.INonGenericClass", actual);
        }

        [Test]

        public void GetTypescriptType_BaseNumberType_Test()
        {
            Assert.AreEqual("number", typeof(short).ToTypeScript());
            Assert.AreEqual("number", typeof(int).ToTypeScript());
            Assert.AreEqual("number", typeof(long).ToTypeScript());
            Assert.AreEqual("number", typeof(float).ToTypeScript());
            Assert.AreEqual("number", typeof(double).ToTypeScript());
            Assert.AreEqual("number", typeof(decimal).ToTypeScript());

        }

        [Test]
        public void GetTypescriptType_Nullable_Test()
        {
            Assert.AreEqual("number", typeof(short?).ToTypeScript());
            Assert.AreEqual("number", typeof(int?).ToTypeScript());
            Assert.AreEqual("number", typeof(long?).ToTypeScript());
            Assert.AreEqual("number", typeof(float?).ToTypeScript());
            Assert.AreEqual("number", typeof(double?).ToTypeScript());
            Assert.AreEqual("number", typeof(decimal?).ToTypeScript());
        }

        [Test]
        public void GetTypescriptType_Enum_Test()
        {
            Assert.AreEqual("FooCountToThree", typeof(FooCountToThree?).ToTypeScript());
            Assert.AreEqual("FooCountToThree", typeof(FooCountToThree).ToTypeScript());
        }

        [Test]
        public void GetTypescriptType_Date_Test()
        {
            Assert.AreEqual("Date", typeof(DateTime).ToTypeScript());
            Assert.AreEqual("Date", typeof(DateTimeOffset).ToTypeScript());
        }

        [Test]
        public void GetTypescriptType_Object_Test()
        {
            Assert.AreEqual("any", typeof(System.Object).ToTypeScript());
            Assert.AreEqual("any", typeof(object).ToTypeScript());
        }

        [Test]
        public void GetTypescriptType_GenericPropertyOfGenericClass()
        {
            var typeDefinition = typeof(GenericClass<int>).GetGenericTypeDefinition();

            var result = typeDefinition.GetProperties()
                                       .Select(property => property.PropertyType.ToTypeScript())
                                       .ToList();
            Assert.IsTrue(result.All(s => s == "TFoo[]"));
        }

    }

    public enum FooCountToThree
    {
        One, Two, Three
    }
}
