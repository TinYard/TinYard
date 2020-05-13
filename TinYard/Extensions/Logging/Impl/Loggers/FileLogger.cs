using System.IO;
using TinYard.Extensions.Logging.API.Interfaces;

namespace TinYard.Extensions.Logging.Impl.Loggers
{
    public class FileLogger : ILogger
    {
        public string LastLogFilePath => _lastLogFilePath;

        private string _fileDestination;
        private string _fileNamePrefix;
        private int _maxLogPerFile;

        private string _lastLogFilePath;


        private const string ERROR_PREFIX = "ERROR: ";
        private const string WARNING_PREFIX = "Warning: ";

        public FileLogger(string fileDestination, string fileNamePrefix = "", int maxLogPerFile = 1000)
        {
            _fileDestination = fileDestination;
            _fileNamePrefix = fileNamePrefix;
            _maxLogPerFile = maxLogPerFile;

            InitialiseLastLogFilePath();
        }

        public void Log(string message)
        {
            Log(string.Empty, message);
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
            string path = GetLogFilePath();
            File.AppendAllLines(path, new string[] { string.Format("{0}{1}", prefix, message) } );
        }

        private string GetLogFilePath()
        {
            //If it doesn't exist, this path is valid
            if(File.Exists(_lastLogFilePath))
            {
                //Check if we've reached max logs for this file, if so - create a new one
                if(File.ReadAllLines(_lastLogFilePath).Length >= _maxLogPerFile)
                {
                    _lastLogFilePath = CreateNewLogFile();
                }
            }

            return _lastLogFilePath;
        }

        private string CreateNewLogFile()
        {
            FileInfo fileInfo = new FileInfo(_lastLogFilePath);
            DirectoryInfo directory = fileInfo.Directory;
            int fileNumber = directory.GetFiles().Length;

            //E.g, 'DATA_log_3'
            string filename = string.Format("{0}log_{1}", _fileNamePrefix, fileNumber);
            string fileExtension = ".txt";

            //Add name and extension together.
            //E.g, 'DATA_log_3.txt'
            string filepath = Path.Combine(directory.FullName, Path.ChangeExtension(filename, fileExtension));


            //If a file exists with that name+extension then we need to find a better name
            int index = 1;
            while(File.Exists(filepath))
            {
                //Add an index to the name to try find a unique one
                //E.g, 'DATA_log_3(1).txt'
                string newName = string.Concat(filename, "(", index, ")");
                filepath = Path.Combine(directory.FullName, Path.ChangeExtension(newName, fileExtension));

                index++;
            }

            //Create the file and close the stream
            File.Create(filepath).Close();

            return filepath;
        }

        private void InitialiseLastLogFilePath()
        {
            //Create the destination if needed
            if (!Directory.Exists(_fileDestination))
                Directory.CreateDirectory(_fileDestination);

            DirectoryInfo directory = new DirectoryInfo(_fileDestination);
            int fileNumber = directory.GetFiles().Length;
            string filename = string.Format("{0}log_{1}", _fileNamePrefix, fileNumber);
            string fileExtension = ".txt";

            _lastLogFilePath = Path.Combine(_fileDestination, Path.ChangeExtension(filename, fileExtension));

            //This function properly ensures this is valid. We're just setting the path variable to be usable
            CreateNewLogFile();
        }
    }
}
