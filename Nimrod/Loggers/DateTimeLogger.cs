using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Nimrod
{
    public class DateTimeLogger : ILogger, IDisposable
    {
        public TraceListener Tracer { get; }
        public DateTimeLogger(TraceListener tracer)
        {
            this.Tracer = tracer.ThrowIfNull(nameof(tracer));
        }

        public void WriteLine(string message) => this.Tracer.WriteLine($"{DateTime.Now.ToLocalTime()} - {message}");

        public void Dispose()
        {
            if (this.Tracer != null)
            {
                this.Tracer.Dispose();
            }
        }
    }
}
