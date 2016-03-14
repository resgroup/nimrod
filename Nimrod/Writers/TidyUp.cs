using System;

namespace Nimrod
{
    public class TidyUp : IDisposable
    {
        private readonly Action _tidyUp;

        public TidyUp(Action tidyUp)
        {
            if (tidyUp == null) throw new ArgumentNullException("tidyUp");
            _tidyUp = tidyUp;
        }

        public TidyUp(Action makeMess, Action tidyUp) : this(tidyUp)
        {
            if (makeMess == null) throw new ArgumentNullException("makeMess");

            makeMess();
        }

        public void Dispose()
        {
            _tidyUp();
        }
    }
}
