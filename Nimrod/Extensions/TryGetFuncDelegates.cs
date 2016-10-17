using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public delegate bool TryGetFunc<in T, TResult>(T arg, out TResult result);
}
