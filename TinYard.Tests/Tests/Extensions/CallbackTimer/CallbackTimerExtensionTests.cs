using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Services;

namespace TinYard.Extensions.CallbackTimer.Tests
{
    [TestClass]
    public class CallbackTimerExtensionTests
    {
        private CallbackTimerExtension _extension;
        private IContext _context;

        [TestInitialize]
        public void Setup()
        {
            _extension = new CallbackTimerExtension();
            _context = new Context();
        }

        [TestCleanup]
        public void Teardown()
        {
            _extension = null;
            _context = null;
        }

        [TestMethod]
        public void CallbackTimerExtension_Is_IExtension()
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
        public void CallbackTimer_Is_Mapped()
        {
            _context.Install(_extension);
            _context.Initialize();

            Assert.IsNotNull(_context.Mapper.GetMappingValue<ICallbackTimer>());
        }
    }
}
