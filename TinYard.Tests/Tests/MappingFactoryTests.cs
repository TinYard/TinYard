using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Factories;
using TinYard.Impl.Mappers;
using TinYard.Impl.VO;
using TinYard.Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class MappingFactoryTests
    {
        private IContext _context;
        private IMapper _mapper;
        private IMappingFactory _mappingFactory;

        private IMappingObject _testingMappingObject;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _mapper = _context.Mapper;
            _mappingFactory = new MappingValueFactory(_mapper);

            _testingMappingObject = _mapper.Map<TestCreatable>().ToSingleton<TestCreatable>();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _mapper = null;
            _mappingFactory = null;

            _testingMappingObject = null;
        }

        [TestMethod]
        public void MappingValueFactory_Is_IMappingFactory()
        {
            Type expected = typeof(IMappingFactory);
            Assert.IsInstanceOfType(_mappingFactory, expected);
        }

        [TestMethod]
        public void MappingValueFactory_Creates_Expected_Type()
        {
            Type expected = typeof(TestCreatable);

            var value = _mappingFactory.Build<TestCreatable>();
            Assert.IsInstanceOfType(value, expected);
        }

        [TestMethod]
        public void Created_Object_Has_Constructor_Dependencies()
        {
            object expected = _context;

            //Context will be mapped to IContext in Mapper that the Factory is using. 
            //TestCreatable has an IContext dependency in constructor
            TestCreatable mappedValue = _mappingFactory.Build<TestCreatable>();
            object actual = mappedValue.Context;

            Assert.AreSame(expected, actual);
        }
    }
}
