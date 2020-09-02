using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.Tests.MockClasses;
using TinYard.Tests.TestClasses;

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
        public void MediatorMapper_Is_IMediatorMapper()
        {
            Assert.IsInstanceOfType(_mapper, typeof(IMediatorMapper));
        }

        [TestMethod]
        public void MediatorMapper_Maps_Mediator_To_View()
        {
            var view = new TestView();
            var expected = new TestMediator();
            _mapper.Map(view).ToMediator(expected);

            var actual = _mapper.GetMapping<TestView>().Mediator;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MediatorMapper_Fetches_View_On_Generic_Map()
        {
            var mediator = new TestMediator();

            var expected = _mapper.Map<TestView>().ToMediator(mediator);

            var actual = _mapper.GetMapping<TestView>();

            Assert.AreEqual(expected, actual);
        }
    }
}
