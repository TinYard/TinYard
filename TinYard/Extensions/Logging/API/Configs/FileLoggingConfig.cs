using System;
using System.IO;
using TinYard.API.Interfaces;
using TinYard.Extensions.Logging.API.Interfaces;
using TinYard.Extensions.Logging.Impl.Loggers;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.Logging.API.Configs
{
    public class FileLoggingConfig : IConfig
    {
        [Inject]
        private IContext _context;

        private FileLogger _fileLogger;

        private string _fileDestination;
        private string _fileNamePrefix;
        private int _maxLogPerFile;

        public void Configure()
        {
            _fileLogger = new FileLogger(_fileDestination, _fileNamePrefix, _maxLogPerFile);

            _context.Mapper.Map<ILogger>().ToValue(_fileLogger);
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
    }
}
