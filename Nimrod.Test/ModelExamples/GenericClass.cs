using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test.ModelExamples
{

    public class GenericClass<TFoo>
    {
        public List<TFoo> GenericList { get; set; }
        public IList<TFoo> GenericIList { get; set; }
        public IEnumerable<TFoo> GenericIEnumerable { get; set; }
    }
}
