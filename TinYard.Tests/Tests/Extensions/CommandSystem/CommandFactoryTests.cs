using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.Factories;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Framework.API.Interfaces;
using TinYard.Tests.TestClasses;

namespace TinYard.Extensions.CommandSystem.Tests
{
    [TestClass]
    public class CommandFactoryTests
    {
        private ICommandFactory _commandFactory;

        [TestInitialize]
        public void Setup()
        {
            _commandFactory = new CommandFactory();
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
    }
}
