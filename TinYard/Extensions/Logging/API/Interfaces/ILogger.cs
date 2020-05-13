namespace TinYard.Extensions.Logging.API.Interfaces
{
    public interface ILogger
    {
        string LastLogFilePath { get; }

        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
    }
}
