using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Demo.Models
{
    public class Movie
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public List<string> Actors { get; set; }
    }
}
