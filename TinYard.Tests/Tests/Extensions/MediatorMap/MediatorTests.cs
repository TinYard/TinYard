using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Extensions.MediatorMap.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorTests
    {
        private Mediator _mediator;

        [TestInitialize]
        public void Setup()
        {
            _mediator = new Mediator();
        }

        [TestCleanup]
        public void Teardown()
        {
            _mediator = null;
        }

        [TestMethod]
        public void Mediator_Is_IMediator()
        {
            Assert.IsInstanceOfType(_mediator, typeof(IMediator));
        }     

        [TestMethod]
        public void Mediator_Is_IEventDispatcher()
        {
            Assert.IsInstanceOfType(_mediator, typeof(IEventDispatcher));
        }

        [TestMethod]
        public void Mediator_Dispatches_Successfully()
        {
            bool listenerInvoked = false;

            _mediator.AddListener(TestEvent.Type.Test1, () =>
            {
                listenerInvoked = true;
            });

            _mediator.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsTrue(listenerInvoked);
        }
    }
}
