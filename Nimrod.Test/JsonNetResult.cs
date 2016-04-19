namespace Nimrod.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Fake JsonNetResult 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class JsonNetResult<T>
    {
        public T Value { get; set; }
    }
}
