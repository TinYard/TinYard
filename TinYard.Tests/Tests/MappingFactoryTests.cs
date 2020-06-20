using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Factories;
using TinYard.Impl.Mappers;
using TinYard.Tests.TestClasses;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class MappingFactoryTests
    {
        private IContext _context;
        private IMapper _mapper;
        private IMappingFactory<TestCreatable> _mappingFactory;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _mapper = _context.Mapper;
            _mappingFactory = new MappingValueFactory<TestCreatable>(_mapper);
        }

        [TestCleanup]
        public void Teardown()
        {

        }

        [TestMethod]
        public void MappingValueFactory_Is_IMappingFactory()
        {
            Type expected = typeof(IMappingFactory<TestCreatable>);
            Assert.IsInstanceOfType(_mappingFactory, expected);
        }

        [TestMethod]
        public void MappingValueFactory_Creates_Expected_Type()
        {
            Type expected = typeof(TestCreatable);

            object value = _mappingFactory.Build();
            Assert.IsInstanceOfType(value, expected);
        }
    }
}
