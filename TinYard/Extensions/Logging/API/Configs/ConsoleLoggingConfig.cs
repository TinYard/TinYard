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

        public object Environment { get { return _environment; } }
        private object _environment;

        public ConsoleLoggingConfig(object environment = null)
        {
            _environment = environment;
        }

        public void Configure()
        {
            ConsoleLogger consoleLogger = new ConsoleLogger();
            mapper.Map<ILogger>().ToValue(consoleLogger);
        }
    }
}
