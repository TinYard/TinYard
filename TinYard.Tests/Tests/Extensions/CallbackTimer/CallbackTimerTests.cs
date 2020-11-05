using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Services;

namespace TinYard.Extensions.CallbackTimer.Tests
{
    [TestClass]
    public class CallbackTimerTests
    {
        private IContext _context;
        private CallbackTimerService _callbackTimer;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _context.Install(new CallbackTimerExtension());
            _context.Initialize();

            _callbackTimer = _context.Mapper.GetMappingValue<ICallbackTimer>() as CallbackTimerService;
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _callbackTimer = null;
        }

        [TestMethod]
        public void CallbackTimer_Is_ICallbackTimer()
        {
            Assert.IsInstanceOfType(_callbackTimer, typeof(ICallbackTimer));
        }

        [TestMethod]
        public void CallbackTimer_Allows_Callback_Adding()
        {
            _callbackTimer.AddTimer(0, () => { });
        }

        [TestMethod]
        public void CallbackTimer_Invokes_Callback()
        {
            bool invoked = false;

            _callbackTimer.AddTimer(0, () =>
            {
                invoked = true;
            });

            //So that we're not relying on System time and whatnot, invoke it directly
            _callbackTimer.UpdateTimers(0d);

            Assert.IsTrue(invoked);
        }
    }
}
