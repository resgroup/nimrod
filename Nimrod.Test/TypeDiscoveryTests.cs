using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Mvc;

namespace Nimrod.Test
{
    public class TypeDiscoveryTests
    {
        public class HttpTestAttribute : Attribute
        {
        }
        public class NoTypescriptDetactableController : Controller
        {
            [HttpTest]
            public JsonNetResult<bool> TestAction(string stringValue)
            {
                throw new NotImplementedException();
            }

            public JsonNetResult<bool> TestAction2(string stringValue)
            {
                throw new NotImplementedException();
            }
        }
        public class TestController : Controller
        {
            [HttpGet]
            public JsonNetResult<bool> TestAction(string stringValue)
            {
                throw new NotImplementedException();
            }
        }

        public class GenericFoo<T>
        {
            T Property { get; set; }
        }

        public class GenericReturnController : Controller
        {
            [HttpGet]
            public JsonNetResult<GenericFoo<int>> TestAction(string stringValue)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void CheckGetControllerActionTypesForActionWithNonRecognizedHttpAttribute()
        {
            var result = TypeDiscovery.GetControllerTypes(typeof(NoTypescriptDetactableController), true, new HashSet<Type>());

            Assert.IsFalse(result.Any());
        }


        [Test]
        public void CheckGetControllerActionTypesForActionWithHttpAttribute()
        {
            var testAction = typeof(TestController).GetMethod("TestAction");

            var result = TypeDiscovery.GetControllerActionParameterTypes(testAction, new HashSet<Type>()).Distinct()
                                      .OrderBy(t => t.Name).ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(typeof(bool), result[0]);
            Assert.AreEqual(typeof(string), result[1]);
        }

        [Test]
        public void CheckGetControllerTypes()
        {
            var result = TypeDiscovery.GetControllerTypes(typeof(TestController), true, new HashSet<Type>()).Distinct()
                                      .OrderBy(t => t.Name)
                                      .ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(typeof(bool), result[0]);
            Assert.AreEqual(typeof(string), result[1]);
            Assert.AreEqual(typeof(TestController), result[2]);
        }

        [Test]
        public void EnumerateTypesTest_string()
        {
            var result = TypeDiscovery.EnumerateTypes(typeof(string), new HashSet<Type>()).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(typeof(string), result[0]);
        }



        public class nothing
        {
        }
        public class GenericContainer
        {
            public GenericFoo<nothing> Property { get; set; }
        }
        [Test]
        public void EnumerateTypes_ShouldReturnGenericContainer()
        {
            var result = TypeDiscovery.EnumerateTypes(typeof(GenericContainer), new HashSet<Type>()).ToList();
            Assert.IsTrue(result.Contains(typeof(GenericFoo<nothing>)));
        }
    }
}
