using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Impl.Mappers;
using TinYard.Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class MappingTests
    {
        private IMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _mapper = new ValueMapper();
        }

        [TestCleanup]
        public void Teardown()
        {
            _mapper = null;
        }

        [TestMethod]
        public void ValueMapper_Is_IMapper()
        {
            Assert.IsInstanceOfType(_mapper, typeof(IMapper));
        }

        [TestMethod]
        public void Mapper_Has_Value_Mapped()
        {
            string expected = "testval";

            _mapper.Map<string>().ToValue(expected);

            string actual = (string)_mapper.GetMappingValue<string>();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mapper_Builds_Value_Correctly()
        {
            Type expected = typeof(TestCreatable);

            TestCreatable actual = (TestCreatable)_mapper.Map<TestCreatable>().BuildValue<TestCreatable>().MappedValue;

            Assert.IsNotNull(actual);
            Assert.IsInstanceOfType(actual, expected);
        }
    }
}