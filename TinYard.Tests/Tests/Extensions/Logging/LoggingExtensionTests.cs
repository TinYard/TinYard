using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.Logging.API.Configs;
using TinYard.Extensions.Logging.API.Interfaces;

namespace TinYard.Extensions.Logging.Tests
{
    [TestClass]
    public class LoggingExtensionTests
    {
        private IContext _context;
        private LoggingExtension _extension;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _extension = new LoggingExtension();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _extension = null;
        }

        [TestMethod]
        public void LoggingExtension_is_IExtension()
        {
            Assert.IsInstanceOfType(_extension, typeof(IExtension));
        }

        [TestMethod]
        public void Context_Installs_Extension()
        {
            _context.Install(_extension);
            _context.Initialize();
        }

        [TestMethod]
        public void File_Logging_Config_Configures_Without_Errors()
        {
            _context.Configure(new FileLoggingConfig());
            _context.Initialize();
        }

        [TestMethod]
        public void File_Logging_Config_Maps_Logger()
        {
            _context.Configure(new FileLoggingConfig());
            _context.Initialize();

            Assert.IsNotNull(_context.Mapper.GetMappingValue<ILogger>());
        }
    }
}