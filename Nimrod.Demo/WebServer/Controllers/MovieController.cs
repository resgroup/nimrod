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
        private static int IdIncrement = 2;
        public readonly static List<Movie> MoviesPersistence = new List<Movie>() {
                new Movie { Id = 1, Name = "Pulp Fiction", Rating= 9, Actors = new List<string>() { "John Travolta", "Samuel L. Jackson"} }
            };


        [HttpGet]
        public JsonNetResult<Movie> Movie(int id)
        {
            var movie = MoviesPersistence.Where(m => m.Id == id).FirstOrDefault();
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
            movie.Id = IdIncrement++;

            MoviesPersistence.Add(movie);
            return JsonNetResult.Create(movie);
        }

        [HttpPost]
        public JsonNetResult<bool> Delete(int id)
        {
            MoviesPersistence.RemoveAll(m => m.Id == id);
            return JsonNetResult.Create(true);
        }
    }
}