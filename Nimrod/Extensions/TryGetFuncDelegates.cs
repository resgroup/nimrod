using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nimrod
{
    public delegate bool TryGetFunc<T, TResult>(T arg, out TResult result);
    public delegate bool TryGetFunc<T1, T2, TResult>(T1 arg1, out TResult result);
    public delegate bool TryGetFunc<T1, T2, T3, TResult>(T1 arg1, T2 arg2, out TResult result);
}
