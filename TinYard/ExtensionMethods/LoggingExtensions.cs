using Microsoft.Extensions.Logging;

namespace TinYard.ExtensionMethods
{
    internal static class LoggingExtensions
    {
        public static void Debug<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }

        public static void Info<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public static void Warning<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }

        public static void Error<T>(this ILogger<T> logger, string message, params object[] args)
        {
            logger.LogError(message, args);
        }
    }
}
