using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod.Console
{
    public class ConsoleLogger : ILogger
    {
        public void WriteLine(string log)
        {
            System.Console.WriteLine($"{DateTime.Now.ToLocalTime()} - {log}");
        }
    }
}
