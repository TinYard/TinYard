using System;
using TinYard.Extensions.Logging.API.Interfaces;

namespace TinYard.Extensions.Logging.Impl.Loggers
{
    public class ConsoleLogger : ILogger
    {
        public string LastLogFilePath => throw new NotImplementedException();

        public void Log(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }
    }
}
