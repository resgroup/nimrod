using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nimrod
{
    public abstract class StaticToTypeScript
    {
        public abstract IEnumerable<string> GetRestApiLines();
        public abstract IEnumerable<string> GetPromiseLines();
    }
}
