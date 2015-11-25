using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Nimrod.Test.ModelExamples
{
    public class IgnoreDataMemberClass
    {
        [IgnoreDataMember]
        public int Foo { get; set; }
    }
}
