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

        [TestMethod]
        public void Mapper_Provides_Named_Mappings_Over_Normal_Order_Dependent()
        {
            string mappingName = "foobar";
            int expected = 1;
            int notExpected = 2;

            //Map two to the int type, and see if the mapper prefers the one with the name - Maybe because it's mapped first
            _mapper.Map<int>(mappingName).ToValue(expected);
            _mapper.Map<int>().ToValue(notExpected);

            var mapping = _mapper.GetMapping<int>(mappingName);
            var mappingValue = _mapper.GetMappingValue<int>(mappingName);

            Assert.IsNotNull(mapping);
            Assert.AreEqual(expected, mappingValue);
            Assert.AreNotEqual(notExpected, mappingValue);
        }

        [TestMethod]
        public void Mapper_Provides_Named_Mappings_Over_Normal_Order_Independent()
        {
            string mappingName = "foobar";
            int expected = 1;
            int notExpected = 2;

            //Map two to the int type, and see if the mapper prefers the one with the name.
            //Map the named one second so that this test ensures the order of mapping isn't relevant
            _mapper.Map<int>().ToValue(notExpected);
            _mapper.Map<int>(mappingName).ToValue(expected);

            var mapping = _mapper.GetMapping<int>(mappingName);
            var mappingValue = _mapper.GetMappingValue<int>(mappingName);

            Assert.IsNotNull(mapping);
            Assert.AreEqual(expected, mappingValue);
            Assert.AreNotEqual(notExpected, mappingValue);
        }

        [TestMethod]
        public void Mapper_Provides_Mapping_From_Value_Environment()
        {
            object env1 = TestEnvironments.Test1;
            object env2 = TestEnvironments.Test2;

            (var unexpectedMapping, var expectedMapping) = CreateEnvironmentMappings<object>(env1, env2);

            //Request mapping from env 2
            var actualMapping = _mapper.GetMapping<object>(env2);

            Assert.AreEqual(expectedMapping, actualMapping);
        }

        [TestMethod]
        public void Mapper_Provides_Mapping_From_Enum_Environment()
        {
            (var unexpectedMapping, var expectedMapping) = CreateEnvironmentMappings<object>(TestEnvironments.Test1, TestEnvironments.Test2);

            //Request mapping from env 2
            var actualMapping = _mapper.GetMapping<object>(TestEnvironments.Test2);

            Assert.AreEqual(expectedMapping, actualMapping);
        }

        [TestMethod]
        public void Mapper_Provides_Mapping_From_Static_Environment()
        {
            (var unexpectedMapping, var expectedMapping) = CreateEnvironmentMappings<object>(StaticTestClass.TestValue, StaticTestClass.OtherTestValue);

            var actualMapping = _mapper.GetMapping<object>(StaticTestClass.OtherTestValue);

            Assert.AreEqual(expectedMapping, actualMapping);
        }

        private (IMappingObject unexpectedMapping, IMappingObject expectedMapping) CreateEnvironmentMappings<T>(object env1, object env2)
        {
            //Create mapping in env 1
            var unexpectedMapping = _mapper.Map<T>(env1).ToValue(env1);

            //Create mapping in env 2
            var expectedMapping = _mapper.Map<T>(env2).ToValue(env2);

            return (unexpectedMapping, expectedMapping); 
        }
    }
}