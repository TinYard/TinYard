using TinYard.Extensions.Logging.API.Interfaces;

namespace TinYard.Extensions.Logging.Impl.Loggers
{
    public class FileLogger : ILogger
    {
        private string _fileDestination;
        private string _fileNamePrefix;
        private int _maxLogPerFile;

        private const string ERROR_PREFIX = "ERROR: ";
        private const string WARNING_PREFIX = "Warning: ";

        public FileLogger(string fileDestination, string fileNamePrefix, int maxLogPerFile)
        {
            _fileDestination = fileDestination;
            _fileNamePrefix = fileNamePrefix;
            _maxLogPerFile = maxLogPerFile;
        }

        public void Log(string message)
        {
            Log("", message);
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

        }
    }
}
