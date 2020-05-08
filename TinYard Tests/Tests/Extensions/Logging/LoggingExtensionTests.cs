using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.Logging;
using TinYard.Impl.Exceptions;
using TinYard.Tests.MockClasses;

namespace TinYard.Tests.Extensions.Logging
{
    [TestClass]
    public class LoggingExtensionTests
    {
        private IContext _context;
        private LoggingExtension _loggingExtension;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
        }

        [TestMethod]
        public void LoggingExtension_is_IExtension()
        {
            Assert.IsInstanceOfType(_loggingExtension, typeof(IExtension));
        }

        [TestMethod]
        public void Context_Installs_Extension()
        {
            _loggingExtension = new LoggingExtension();
            _context.Install(_loggingExtension);
            _context.Initialize();
            //Assert nothing, if it doesn't throw it's a success
        }
    }
}
