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
        public void File_Logging_Creates_File_In_Correct_Place()
        {
            Assert.IsTrue(_testDirectory.GetFiles().Length == 0);
            _logger = new FileLogger(_testDestination);
            Assert.IsTrue(_testDirectory.GetFiles().Length > 0);
        }
    }
}