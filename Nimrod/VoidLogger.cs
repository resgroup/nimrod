using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class VoidLogger : ILogger
    {
        public static readonly VoidLogger Default = new VoidLogger();
        public void WriteLine(string log)
        {
            // void logger doesn't log
        }
    }
}
