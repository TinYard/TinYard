using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.Factories;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Framework.API.Interfaces;
using TinYard.Tests.TestClasses;
using TinYard_Tests.TestClasses;

namespace TinYard.Extensions.CommandSystem.Tests
{
    [TestClass]
    public class CommandFactoryTests
    {
        private ICommandFactory _commandFactory;
        private IContext _context;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _commandFactory = new CommandFactory(_context.Injector);
        }

        [TestCleanup]
        public void Teardown()
        {
            _commandFactory = null;
        }

        [TestMethod]
        public void CommandFactory_Is_IFactory()
        {
            Assert.IsInstanceOfType(_commandFactory, typeof(IFactory));
        }

        [TestMethod]
        public void CommandFactory_Is_ICommandFactory()
        {
            Assert.IsInstanceOfType(_commandFactory, typeof(ICommandFactory));
        }

        [TestMethod]
        public void CommandFactory_Creates_Command()
        {
            Assert.IsNotNull(_commandFactory.Build<TestCommand>());
        }

        [TestMethod]
        public void CommandFactory_Creates_Correct_Command()
        {
            Type expected = typeof(TestCommand);
            var actual = _commandFactory.Build<TestCommand>();
            Assert.IsInstanceOfType(actual, expected);
        }

        [TestMethod]
        public void CommandFactory_Injects_Command()
        {
            _context.Mapper.Map<TestInjectable>().ToValue(new TestInjectable());

            TestEvent tEvent = new TestEvent(TestEvent.Type.Test1);
            var command = _commandFactory.Build<TestCommand>(tEvent);

            Assert.IsNotNull(command.Evt);
            Assert.IsNotNull(command.Injectable);
        }

        [TestMethod]
        public void CommandFactory_Injects_Command_Correctly()
        {
            var expectedInjectable = new TestInjectable();
            _context.Mapper.Map<TestInjectable>().ToValue(expectedInjectable);

            TestEvent expectedEvent = new TestEvent(TestEvent.Type.Test1);
            var command = _commandFactory.Build<TestCommand>(expectedEvent);

            Assert.AreEqual(expectedEvent, command.Evt);
            Assert.AreEqual(expectedInjectable, command.Injectable);
        }
    }
}
