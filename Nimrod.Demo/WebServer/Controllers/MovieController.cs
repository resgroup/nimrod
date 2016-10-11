using Nimrod.Demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Nimrod.Demo.Controllers
{

    public class MovieController : Controller
    {
        // fake database
        public readonly static List<Movie> MoviesPersistence = new List<Movie> {
                new Movie {
                    Guid = Guid.NewGuid().ToString(),
                    Name = "Pulp Fiction",
                    Rating = 9,
                    Actors = new List<string> { "John Travolta", "Samuel L. Jackson"}
                }
            };

        [HttpGet]
        public JsonNetResult<Movie> Movie(string guid)
        {
            var movie = MoviesPersistence.FirstOrDefault(m => m.Guid == guid);
            return JsonNetResult.Create(movie);
        }

        [HttpGet]
        public JsonNetResult<List<Movie>> Movies()
        {
            var movies = MoviesPersistence;
            return JsonNetResult.Create(movies);
        }

        [HttpPost]
        public JsonNetResult<Movie> Add(Movie movie)
        {
            movie.Guid = Guid.NewGuid().ToString();

            MoviesPersistence.Add(movie);
            return JsonNetResult.Create(movie);
        }

        [HttpPost]
        public JsonNetResult<bool> Delete(string guid)
        {
            MoviesPersistence.RemoveAll(m => m.Guid == guid);
            return JsonNetResult.Create(true);
        }
    }
}