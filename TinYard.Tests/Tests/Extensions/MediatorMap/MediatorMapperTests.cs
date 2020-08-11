using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorMapperTests
    {
        private MediatorMapper _mapper;

        [TestInitialize]
        public void Setup()
        {
            _mapper = new MediatorMapper();
        }

        [TestCleanup]
        public void Teardown()
        {
            _mapper = null;
        }

        [TestMethod]
        public void MediatorMapper_Is_IMapper()
        {
            Assert.IsInstanceOfType(_mapper, typeof(IMapper));
        }

        [TestMethod]
        public void MediatorMapper_Is_IMediatorMapper()
        {
            Assert.IsInstanceOfType(_mapper, typeof(IMediatorMapper));
        }
    }
}
