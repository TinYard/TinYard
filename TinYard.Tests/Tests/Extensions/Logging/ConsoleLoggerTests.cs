using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using TinYard.Extensions.Logging.API.Interfaces;
using TinYard.Extensions.Logging.Impl.Loggers;

namespace TinYard.Extensions.Logging.Tests
{
    [TestClass]
    public class ConsoleLoggerTests
    {
        private ILogger _logger;

        [TestInitialize]
        public void Setup()
        {
            _logger = new ConsoleLogger();
        }

        [TestCleanup]
        public void Teardown()
        {
            _logger = null;
        }

        [TestMethod]
        public void Console_Logger_is_ILogger()
        {
            Type expected = typeof(ILogger);
            Assert.IsInstanceOfType(_logger, expected);
        }

        [TestMethod]
        public void Outputs_to_Console_Stream()
        {
            //Create an output for console logs that we can access
            StringWriter consoleOut = new StringWriter();

            //Make sure that `writeLine` doesn't add any extra characters
            consoleOut.NewLine = "";
            Console.SetOut(consoleOut);

            string expected = "1";
            _logger.Log(expected);

            string actual = consoleOut.ToString();

            Assert.AreEqual(expected, actual);        
        }
    }
}
