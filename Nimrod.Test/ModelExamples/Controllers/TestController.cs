using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nimrod.Test.ModelExamples
{
    public class TestController : Controller
    {
        

        [HttpGet]
        public JsonNetResult<bool> TestAction(string stringValue)
        {
            throw new NotImplementedException();
        }
    }
}
