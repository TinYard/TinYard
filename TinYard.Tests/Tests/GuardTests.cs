using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class GuardTests
    {
        private IContext _context;
        private ICommandMap _commandMap;
        private IEventDispatcher _eventDispatcher;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _commandMap = null;
        }

        [TestMethod]
        public void Guard_Can_Be_Added_To_Command()
        {
            SetupCommandExtension();

            _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestCommand>().WithGuard<TestGuard>();
        }

        [TestMethod]
        public void Command_Doesnt_Run_If_Guard_Not_Satisfied()
        {
            SetupCommandExtension();

            _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestGuardedCommand>().WithGuard<TestGuard>();

            _eventDispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsFalse(TestGuardedCommand.ExecuteCalled);
        }

        [TestMethod]
        public void Command_Runs_When_Guard_Satisfied()
        {
            SetupCommandExtension();

            _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestGuardedCommand>().WithGuard<TestGuard>();

            _context.Mapper.Map<object>().ToValue(new object());

            _eventDispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsTrue(TestGuardedCommand.ExecuteCalled);
        }

        private void SetupCommandExtension()
        {
            _context.Install(new EventSystemExtension());
            _context.Install(new CommandSystemExtension());
            _context.Initialize();

            _commandMap = _context.Mapper.GetMappingValue<ICommandMap>() as ICommandMap;
            _eventDispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>() as IEventDispatcher;
        }
    }
}