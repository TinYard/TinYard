using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Runtime.InteropServices;
using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;
using TinYard.Extensions.ViewController.Tests.MockClasses;
using TinYard.Tests.TestClasses;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorMapperTests
    {
        private MediatorMapper _mapper;
        private IContext _context;
        private IViewRegister _viewRegister;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _context.Install(new ViewControllerExtension());
            _context.Mapper.Map<IEventDispatcher>().ToValue(new EventSystem.Impl.EventDispatcher(_context));
            _context.Initialize();
            
            _viewRegister = _context.Mapper.GetMappingValue<IViewRegister>() as IViewRegister;
            
            _mapper = new MediatorMapper(_context, _viewRegister);
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

        [TestMethod]
        public void MediatorMapper_Maps_Two_Generics_Without_Throwing()
        {
            _mapper.Map<TestView>().ToMediator<TestMediator>();

            TestView view = new TestView();
        }
    }
}
