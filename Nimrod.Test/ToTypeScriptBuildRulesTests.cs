using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod.Test
{
    [TestFixture]
    public class ToTypeScriptBuildRulesTests
    {
        /// <summary>
        /// Protect the addition of new modules
        /// Alert that build rules have to be implemented
        /// </summary>
        [Test]
        public void GetRules_AllEnumsAreDefined()
        {
            var result = ObjectExtensions.GetEnumValues<ModuleType>()
                    .Select(type => ToTypeScriptBuildRules.GetRules(type))
                    .ToList();
            Assert.NotNull(result);
        }

    }
}
