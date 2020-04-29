using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Impl.Mappers;

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
            string value = "testval";

            _mapper.Map<string>().ToValue(value);

            string val2 = (string)_mapper.GetMappingValue<string>();
            Assert.AreEqual(value, val2);
        }
    }
}