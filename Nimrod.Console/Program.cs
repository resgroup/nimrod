using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Console
{
    static class Program
    {
        static int Main(string[] args)
        {
            return OptionsHandler.Handle(args);
        }
    }
}
