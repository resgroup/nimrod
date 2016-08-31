using Nimrod.Test.ModelExamples;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod.Test.Extensions
{
    [TestFixture]
    public class MethodExtensionsTests
    {
        [Test]
        public void CheckGetControllerActionTypesForActionWithHttpAttribute()
        {
            var testAction = typeof(TestController).GetMethod("TestAction");

            var result = testAction.GetReturnTypeAndParameterTypes()
                                   .Distinct()
                                   .OrderBy(t => t.Name)
                                   .ToList();

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual(typeof(bool), result[0]);
            Assert.AreEqual(typeof(string), result[1]);
        }
    }
}
