using TinYard.API.Interfaces;
using TinYard.Extensions.Logging.API.Interfaces;
using TinYard.Extensions.Logging.Impl.Loggers;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.Logging.API.Configs
{
    public class FileLoggingConfig : IConfig
    {
        [Inject]
        public IContext _context;

        public object Environment { get { return _environment; } }
        private object _environment;

        private FileLogger _fileLogger;

        private string _fileDestination;
        private string _fileNamePrefix;
        private int _maxLogPerFile = 1000;

        public FileLoggingConfig(object environment = null)
        {
            _environment = environment;
        }

        public void Configure()
        {
            if (string.IsNullOrEmpty(_fileDestination))
            {
                SetDefaultFileDestination();
            }

            if(string.IsNullOrEmpty(_fileNamePrefix))
            {
                SetDefaultFileNamePrefix();
            }

            _fileLogger = new FileLogger(_fileDestination, _fileNamePrefix, _maxLogPerFile);

            _context.Mapper.Map<ILogger>().ToSingleton(_fileLogger);
        }

        public FileLoggingConfig WithFileDestination(string destination)
        {
            _fileDestination = destination;

            return this;
        }

        public FileLoggingConfig WithFileNamePrefix(string prefix)
        {
            _fileNamePrefix = prefix;

            return this;
        }

        public FileLoggingConfig WithMaxLogPerFile(int maxLogs)
        {
            _maxLogPerFile = maxLogs;

            return this;
        }

        private void SetDefaultFileDestination()
        {
            _fileDestination = System.Environment.CurrentDirectory;
        }
        
        private void SetDefaultFileNamePrefix()
        {
            _fileNamePrefix = string.Empty;
        }
    }
}
