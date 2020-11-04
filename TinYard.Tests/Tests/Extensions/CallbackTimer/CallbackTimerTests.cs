using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Services;

namespace TinYard.Extensions.CallbackTimer.Tests
{
    [TestClass]
    public class CallbackTimerTests
    {
        private IContext _context;
        private ICallbackTimer _callbackTimer;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _context.Install(new CallbackTimerExtension());
            _context.Initialize();

            _callbackTimer = _context.Mapper.GetMappingValue<ICallbackTimer>();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
        }

        [TestMethod]
        public void CallbackTimer_Is_ICallbackTimer()
        {
            Assert.IsInstanceOfType(_callbackTimer, typeof(ICallbackTimer));
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
