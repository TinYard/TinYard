using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using TinYard.Extensions.Logging.Impl.Loggers;

namespace TinYard.Extensions.Logging.Tests
{
    [TestClass]
    public class FileLoggerTests
    {
        private FileLogger _logger;

        private string _testDestination = @"c:\temp\TinYard\Tests\";
        private DirectoryInfo _testDirectory;

        [TestInitialize]
        public void Setup()
        {
            if (!Directory.Exists(_testDestination))
                Directory.CreateDirectory(_testDestination);

            _testDirectory = new DirectoryInfo(_testDestination);
        }

        [TestCleanup]
        public void Teardown()
        {
            _logger = null;

            //Delete everything in the directory, then the directory itself
            _testDirectory.Delete(true);
        }

        [TestMethod]
        public void File_Logging_Creates_File_In_Correct_Location()
        {
            int filesInDirectory = _testDirectory.GetFiles().Length;
            _logger = new FileLogger(_testDestination);
            Assert.IsTrue(_testDirectory.GetFiles().Length > filesInDirectory);
        }

        [TestMethod]
        public void File_Logger_Adds_Log_To_File()
        {
            _logger = new FileLogger(_testDestination);

            int logFileLinesLength = File.ReadAllLines(_logger.LastLogFilePath).Length;

            _logger.Log("Test log");

            Assert.AreNotEqual(logFileLinesLength, File.ReadAllLines(_logger.LastLogFilePath).Length);
        }

        [TestMethod]
        public void File_Logger_Creates_New_File_At_Max_Lines()
        {
            int maxLines = 5;
            _logger = new FileLogger(_testDestination, string.Empty, maxLines);

            int filesInDirectory = _testDirectory.GetFiles().Length;

            //<= because we want to go one over
            for(int i = 0; i <= maxLines; i++)
            {
                _logger.Log(i.ToString());
            }

            Assert.AreNotEqual(filesInDirectory, _testDirectory.GetFiles().Length);
        }
    }
}