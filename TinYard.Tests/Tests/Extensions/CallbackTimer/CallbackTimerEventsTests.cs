using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer;
using TinYard.Extensions.CallbackTimer.API.Events;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Services;
using TinYard.Extensions.CommandSystem;
using TinYard.Extensions.EventSystem;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.CallbackTimer.Tests
{
    [TestClass]
    public class CallbackTimerEventsTests
    {
        private IContext _context;
        private IEventDispatcher _contextEventDispatcher;
        private CallbackTimerService _callbackTimer;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _context.Install(new EventSystemExtension());
            _context.Install(new CommandSystemExtension());
            _context.Install(new CallbackTimerExtension());

            _context.Initialize();

            _callbackTimer = _context.Mapper.GetMappingValue<ICallbackTimer>() as CallbackTimerService;
            _contextEventDispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _callbackTimer = null;
        }

        [TestMethod]
        public void Can_Add_Timer_Through_Events()
        {
            _contextEventDispatcher.Dispatch(new AddCallbackTimerEvent(AddCallbackTimerEvent.Type.Add, 0, () => { }));
        }

        [TestMethod]
        public void Event_Added_Timer_Gets_Added_And_Invoked()
        {
            bool invoked = false;
            Action callback = () =>
            {
                invoked = true;
            };

            _contextEventDispatcher.Dispatch(new AddCallbackTimerEvent(AddCallbackTimerEvent.Type.Add, 0d, callback));

            _callbackTimer.UpdateTimers(0d);

            Assert.IsTrue(invoked);
        }

        [TestMethod]
        public void Can_Remove_Timer_Through_Events()
        {
            _contextEventDispatcher.Dispatch(new RemoveCallbackTimerEvent(RemoveCallbackTimerEvent.Type.Remove, () => { }));
        }

        [TestMethod]
        public void Event_Removed_Timer_Is_Removed()
        {
            bool invoked = false;
            Action callback = () =>
            {
                invoked = true;
            };

            _callbackTimer.AddTimer(100d, callback);

            _contextEventDispatcher.Dispatch(new RemoveCallbackTimerEvent(RemoveCallbackTimerEvent.Type.Remove, callback));

            _callbackTimer.UpdateTimers(100d);

            Assert.IsFalse(invoked);
        }
    }
}
