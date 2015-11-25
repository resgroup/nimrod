using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Nimrod.Test.ModelExamples
{
    public class Movie
    {
        public string Name { get; }
        public double Rating { get; }
        public List<string> Actors { get; }
    }
    public class MovieController : Controller
    {
        [HttpGet]
        public JsonNetResult<Movie> Movie(int id)
        {
            throw new NotImplementedException();
        }
    }
}
