using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Test.ModelExamples
{
    public class GenericContainer2<Foo2>
    { 
    }
    public class GenericContainer1<Foo1>
    {
        public GenericContainer2<Foo1> Items { get; set; }
    }
}