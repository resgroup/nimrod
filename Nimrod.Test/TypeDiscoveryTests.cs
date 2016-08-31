using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Web.Mvc;
using Nimrod.Test.ModelExamples;

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


        public class GenericFoo<T>
        {
            public T Property { get; set; }
        }

        public class GenericReturnController : Controller
        {
            [HttpGet]
            public JsonNetResult<GenericFoo<int>> TestAction(string stringValue)
            {
                throw new NotImplementedException();
            }
        }

        public class WebApiController : System.Web.Http.ApiController
        {
            [System.Web.Http.HttpGet]
            public string TestAction()
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void CheckGetControllerActionTypesForActionWithNonRecognizedHttpAttribute()
        {
            var result = TypeDiscovery.SeekTypesFromController(typeof(NoTypescriptDetactableController)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(typeof(NoTypescriptDetactableController), result[0]);
        }

        [Test]
        public void CheckGetControllerTypes()
        {
            var result = TypeDiscovery.SeekTypesFromController(typeof(TestController))
                                      .Distinct()
                                      .OrderBy(t => t.Name)
                                      .ToList();

            Assert.AreEqual(3, result.Count);
            Assert.AreEqual(typeof(bool), result[0]);
            Assert.AreEqual(typeof(string), result[1]);
            Assert.AreEqual(typeof(TestController), result[2]);
        }

        [Test]
        public void CheckGetWebApiControllerTypes()
        {
            var result = TypeDiscovery.SeekTypesFromController(typeof(WebApiController))
                                      .Distinct()
                                      .OrderBy(t => t.Name)
                                      .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(typeof(string), result[0]);
            Assert.AreEqual(typeof(WebApiController), result[1]);
        }

        [Test]
        public void EnumerateTypesTest_string()
        {
            var result = TypeDiscovery.EnumerateTypes(typeof(string)).ToList();

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(typeof(string), result[0]);
        }



        public class Nothing
        {
        }
        public class GenericContainer
        {
            public GenericFoo<Nothing> Property { get; set; }
        }
        [Test]
        public void EnumerateTypes_ShouldReturnGenericContainer()
        {
            var result = TypeDiscovery.EnumerateTypes(typeof(GenericContainer)).ToList();
            Assert.IsTrue(result.Contains(typeof(GenericFoo<Nothing>)));
        }

        [Test]
        public void GetBaseTypesTests()
        {
            var result = TypeDiscovery.GetBaseTypes(typeof(Duck)).ToList();
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(typeof(Animal), result.Single());
        }
    }
    public abstract class Animal
    {

    }

    public class Duck : Animal
    {


    }

}
