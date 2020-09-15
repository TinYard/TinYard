using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.EventSystem.Impl.Exceptions;
using TinYard.Extensions.EventSystem.Tests.MockClasses;

namespace TinYard.Extensions.EventSystem.Tests
{
    [TestClass]
    public class EventDispatcherTests
    {
        private IEventDispatcher _eventDispatcher;

        [TestInitialize]
        public void Setup()
        {
            _eventDispatcher = new EventDispatcher();
        }

        [TestCleanup]
        public void Teardown()
        {
            _eventDispatcher = null;
        }

        [TestMethod]
        public void EventDispatcher_Is_IEventDispatcher()
        {
            Assert.IsInstanceOfType(_eventDispatcher, typeof(IEventDispatcher));
        }

        [TestMethod]
        public void EventDispatcher_Doesnt_Throw_On_Add_Listener()
        {
            _eventDispatcher.AddListener(TestEvent.Type.Test1, null);
        }

        [TestMethod]
        public void EventDispatcher_Doesnt_Throw_On_Remove_Listener()
        {
            _eventDispatcher.AddListener(TestEvent.Type.Test1, null);
            _eventDispatcher.RemoveListener(TestEvent.Type.Test1, null);

            _eventDispatcher.RemoveListener(TestEvent.Type.Test2, null);
        }

        [TestMethod]
        public void EventDispatcher_Invokes_Parameterless_Callback()
        {
            bool callbackInvoked = false;

            _eventDispatcher.AddListener(TestEvent.Type.Test1, () =>
            {
                callbackInvoked = true;
            });

            TestEvent testEvent = new TestEvent(TestEvent.Type.Test1);
            _eventDispatcher.Dispatch(testEvent);

            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        public void EventDispatcher_Invokes_Callback_With_Parameter()
        {
            TestEvent testEvent = new TestEvent(TestEvent.Type.Test1);
            bool callbackInvoked = false;

            _eventDispatcher.AddListener<TestEvent>(TestEvent.Type.Test1, (evt) =>
            {
                Assert.AreEqual(testEvent, evt);
                callbackInvoked = true;
            });

            _eventDispatcher.Dispatch(testEvent);

            Assert.IsTrue(callbackInvoked);
        }

        [TestMethod]
        public void EventDispatcher_Invokes_Correct_Callbacks()
        {
            bool incorrectCallbackInvoked = false;
            bool callbackInvoked = false;

            _eventDispatcher.AddListener(TestEvent.Type.Test1, () =>
            {
                callbackInvoked = true;
            });

            _eventDispatcher.AddListener(TestEvent.Type.Test2, () =>
            {
                incorrectCallbackInvoked = true;
            });

            TestEvent testEvent = new TestEvent(TestEvent.Type.Test1);
            _eventDispatcher.Dispatch(testEvent);

            Assert.IsTrue(callbackInvoked);
            Assert.IsFalse(incorrectCallbackInvoked);
        }

        [TestMethod]
        public void EventDispatcher_Throws_On_Invalid_Event_Type()
        {
            Assert.ThrowsException<EventTypeException>(() =>
           {
               _eventDispatcher.AddListener(null, () => { });
           });
        }
    }
}
