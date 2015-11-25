using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test.ModelExamples
{
    public class Paginator<T>
    {
        /// <summary>
        /// Current Page number, 1-indexed base
        /// </summary>
        public int Page { get; }
        

        public List<T> Results { get; }

        /// <summary>
        /// Total number of items to paginate
        /// </summary>
        public int TotalItems => 0;

        /// <summary>
        /// Number of items allowed on a single page
        /// </summary>
        public int? ItemsPerPage { get; }

    }
}


