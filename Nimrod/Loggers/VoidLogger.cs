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
