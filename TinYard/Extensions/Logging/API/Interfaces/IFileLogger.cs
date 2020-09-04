namespace TinYard.Extensions.Logging.API.Interfaces
{
    public interface IFileLogger : ILogger
    {
        string LastLogFilePath { get; }
    }
}
