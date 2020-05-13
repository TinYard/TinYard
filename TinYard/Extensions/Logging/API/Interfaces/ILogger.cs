namespace TinYard.Extensions.Logging.API.Interfaces
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
    }
}
