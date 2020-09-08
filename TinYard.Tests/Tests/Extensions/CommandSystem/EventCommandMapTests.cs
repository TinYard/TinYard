using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.CommandMaps;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Tests.TestClasses;

namespace TinYard.Extensions.CommandSystem.Tests
{
    [TestClass]
    public class EventCommandMapTests
    {
        private EventCommandMap _commandMap;
        private EventDispatcher _dispatcher;
        private IContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _dispatcher = new EventDispatcher(_context);

            _commandMap = new EventCommandMap(_dispatcher, _context.Injector);
        }

        [TestCleanup]
        public void Teardown()
        {
            _commandMap = null;
        }

        [TestMethod]
        public void EventCommandMap_Is_ICommandMap()
        {
            Assert.IsInstanceOfType(_commandMap, typeof(ICommandMap));
        }

        [TestMethod]
        public void EventCommandMap_Creates_Command_Mapping()
        {
            var mapping = _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestCommand>();

            Assert.IsNotNull(mapping);
        }

        [TestMethod]
        public void EventCommandMap_Creates_Command_On_Event()
        {
            _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestCommand>();

            Assert.IsFalse(TestCommand.ExecuteCalled);

            _dispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsTrue(TestCommand.ExecuteCalled);
        }
    }
}
