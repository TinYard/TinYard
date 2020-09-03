using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Tests.TestClasses;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorTests
    {
        private TestMediator _mediator;

        [TestInitialize]
        public void Setup()
        {
            _mediator = new TestMediator();
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
        public void Mediator_Dispatches_Successfully()
        {
            //Inject a dispatcher into the Test Mediator
            Context context = new Context();
            IEventDispatcher dispatcher = new EventDispatcher();
            context.Mapper.Map<IEventDispatcher>().ToValue(dispatcher);
            context.Injector.Inject(_mediator, dispatcher);

            bool listenerInvoked = false;

            _mediator.Dispatcher.AddListener(TestEvent.Type.Test1, () =>
            {
                listenerInvoked = true;
            });

            _mediator.Dispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsTrue(listenerInvoked);
        }
    }
}
