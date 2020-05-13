using TinYard.API.Interfaces;
using TinYard.Extensions.Logging.API.Interfaces;
using TinYard.Extensions.Logging.Impl.Loggers;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.Logging.API.Configs
{
    public class FileLoggingConfig : IConfig
    {
        [Inject]
        private IMapper _contextMapper;

        public void Configure()
        {
            _contextMapper.Map<ILogger>().ToValue(new FileLogger());
        }
    }
}
