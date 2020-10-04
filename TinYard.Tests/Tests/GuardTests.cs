using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.CommandMaps;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.Mappers;
using TinYard.Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class GuardTests
    {
        private IContext _context;
        private ICommandMap _commandMap;

        [TestInitialize]
        public void Setup()
        {
            _context.Install(new CommandSystemExtension());
            _context.Initialize();

            _commandMap = _context.Mapper.GetMappingValue<ICommandMap>() as ICommandMap;
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
            _commandMap.Map<TestEvent>(TestEvent.Type.Test1).ToCommand<TestCommand>().WithGuard<TestGuard>();
        }
    }
}