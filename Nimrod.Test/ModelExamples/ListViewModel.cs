using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test.ModelExamples
{
    public class ListViewModel<T>
    {
        public ListViewModel()
        {
            Items = new List<T>();
        }

        public T AddItemTemplate { get; set; }

        public List<T> Items { get; set; }

        public Paginator<T> Paginator { get; set; }
    }
}
