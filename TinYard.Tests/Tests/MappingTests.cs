using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.Mappers;
using TinYard.Impl.VO;
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
            //Need a MockInjector for the internal Factory
            _mapper.Map<IInjector>().ToValue(new MockInjector());
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

            string actual = _mapper.GetMappingValue<string>();
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

        [TestMethod]
        public void MappingObject_Build_Functionality_Can_Be_Overwritten()
        {
            int expected = 4096;

            Action<IMappingObject, Type> overrideBuild = new Action<IMappingObject, Type>((mappingObj, valType) =>
            {
                mappingObj.ToValue(expected);
            });

            MappingObject mappingObject = new MappingObject();
            mappingObject.BuildDelegate = overrideBuild;
            mappingObject.Map<int>();
            mappingObject.BuildValue<int>();

            Assert.AreEqual(expected, mappingObject.MappedValue);
        }

        [TestMethod]
        public void Mapping_Can_Be_Given_Name()
        {
            _mapper.Map<int>("foobar");
        }

        [TestMethod]
        public void Mapper_Provides_Named_Mappings()
        {
            string mappingName = "foobar";
            int expected = 1;

            _mapper.Map<int>(mappingName).ToValue(expected);
            
            var mapping = _mapper.GetMapping<int>(mappingName);
            var mappingValue = _mapper.GetMappingValue<int>(mappingName);
            
            Assert.IsNotNull(mapping);
            Assert.AreEqual(expected, mappingValue);
        }
    }
}