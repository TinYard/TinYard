using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Factories;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class MappingFactoryTests
    {
        private IMappingFactory<TestInjectable> _mappingFactory;

        [TestInitialize]
        public void Setup()
        {
            _mappingFactory = new MappingValueFactory<TestInjectable>();
        }

        [TestCleanup]
        public void Teardown()
        {

        }

        [TestMethod]
        public void MappingValueFactory_Is_IMappingFactory()
        {
            Type expected = typeof(IMappingFactory<TestInjectable>);
            Assert.IsInstanceOfType(_mappingFactory, expected);
        }

        [TestMethod]
        public void MappingValueFactory_Creates_Expected_Type()
        {
            Type expected = typeof(TestInjectable);

            object value = _mappingFactory.Build();
            Assert.IsInstanceOfType(value, expected);
        }
    }
}
