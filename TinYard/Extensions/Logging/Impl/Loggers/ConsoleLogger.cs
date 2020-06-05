using System;
using TinYard.Extensions.Logging.API.Interfaces;

namespace TinYard.Extensions.Logging.Impl.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private const string WARNING_PREFIX = "Warning:";
        private const string ERROR_PREFIX = "ERROR:";

        public void Log(string message)
        {
            Log(string.Empty, message);
        }

        public void LogWarning(string message)
        {
            Log(WARNING_PREFIX, message);
        }

        public void LogError(string message)
        {
            Log(ERROR_PREFIX, message);
        }

        private void Log(string prefix, string message)
        {
            Console.WriteLine(string.Format("{0}{1}", prefix, message));
        }
    }
}
