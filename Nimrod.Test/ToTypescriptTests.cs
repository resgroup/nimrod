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
        public void GetTypescriptType_ArrayLike_Test()
        {
            Assert.AreEqual("string[]", typeof(List<string>).ToTypeScript());
            Assert.AreEqual("string[]", typeof(IEnumerable<string>).ToTypeScript());
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
            Assert.AreEqual("{ [id: string] : IException[]; }", typeof(Dictionary<string, List<Exception>>).ToTypeScript());
        }

        public class Generic<T>
        {
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
        public void GetTypescriptType_NonGeneric_Test()
        {
            var actual = typeof(NonGenericClass).ToTypeScript(true);
            Assert.AreEqual("Nimrod.Test.INonGenericClass", actual);
        }


        [Test]
        public void GetTypescriptType_Enum_Test()
        {
            Assert.AreEqual("IFooEnum", typeof(FooEnum).ToTypeScript());
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
            Assert.AreEqual("number", typeof(int?).ToTypeScript());
            Assert.AreEqual("number", typeof(float?).ToTypeScript());
            Assert.AreEqual("IFooEnum", typeof(FooEnum?).ToTypeScript());
        }


        [Test]
        public void GetTypescriptType_GenericPropertyOfGenericClass()
        {
            var typeDefinition = typeof(GenericClass<int>).GetGenericTypeDefinition();

            foreach (var property in typeDefinition.GetProperties())
            {
                var ts = property.PropertyType.ToTypeScript();
                Assert.AreEqual("TFoo[]", ts);
            }

        }

    }

    public enum FooEnum
    {
        One, Two, Three
    }
}
