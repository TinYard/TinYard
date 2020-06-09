using TinYard.API.Interfaces;
using TinYard.Extensions.Logging.API.Interfaces;
using TinYard.Extensions.Logging.Impl.Loggers;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.Logging.API.Configs
{
    public class ConsoleLoggingConfig : IConfig
    {
        [Inject]
        public IMapper mapper;

        public void Configure()
        {
            ConsoleLogger consoleLogger = new ConsoleLogger();
            mapper.Map<ILogger>().ToValue(consoleLogger);
        }
    }
}
