using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Factories;
using TinYard.Framework.API.Interfaces;
using TinYard.Tests.TestClasses;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorFactoryTests
    {
        private IMediatorFactory _factory;

        [TestInitialize]
        public void Setup()
        {
            _factory = new MediatorFactory();
        }

        [TestCleanup]
        public void Teardown()
        {
            _factory = null;
        }

        [TestMethod]
        public void MediatorFactory_Is_IFactory()
        {
            Assert.IsInstanceOfType(_factory, typeof(IFactory));
        }

        [TestMethod]
        public void MediatorFactory_Builds_Mediator()
        {
            Assert.IsNotNull(_factory.Build<TestMediator>());
        }

        [TestMethod]
        public void MediatorFactory_Builds_Correct_Type()
        {
            Type expected = typeof(TestMediator);

            Type actual = _factory.Build<TestMediator>().GetType();

            Assert.AreEqual(expected, actual);
        }
    }
}
