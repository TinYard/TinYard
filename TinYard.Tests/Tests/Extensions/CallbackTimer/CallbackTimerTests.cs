using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Services;

namespace TinYard.Extensions.CallbackTimer.Tests
{
    [TestClass]
    public class CallbackTimerTests
    {
        private CallbackTimerService _callbackTimer;

        [TestInitialize]
        public void Setup()
        {
            _callbackTimer = new CallbackTimerService();
        }

        [TestCleanup]
        public void Teardown()
        {
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

        [TestMethod]
        public void CallbackTimer_Allows_Callback_Removing()
        {
            bool invoked = false;

            Action callback = () =>
            {
                invoked = true;
            };

            _callbackTimer.AddTimer(100d, callback);
            _callbackTimer.RemoveTimer(callback);

            _callbackTimer.UpdateTimers(100d);

            Assert.IsFalse(invoked);
        }
    }
}
