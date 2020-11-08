using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.ViewController;
using TinYard.Extensions.ViewController.API.Interfaces;
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
            
            _viewRegister = _context.Mapper.GetMappingValue<IViewRegister>();
            
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

            var actual = _mapper.GetMappings<TestView>().ToArray()[0].Mediator;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MediatorMapper_Fetches_View_On_Generic_Map()
        {
            var mediator = new TestMediator();

            var expected = _mapper.Map<TestView>().ToMediator(mediator);

            var actual = _mapper.GetMappings<TestView>().ToArray()[0];

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void MediatorMapper_Maps_Two_Generics_Without_Throwing()
        {
            _mapper.Map<TestView>().ToMediator<TestMediator>();

            TestView view = new TestView();
        }

        [TestMethod]
        public void Mediator_Can_Map_To_Interface()
        {
            _mapper.Map<ITestView>().ToMediator<InterfaceTestMediator>();

            //ViewRegister would potentially cause this next line to throw if the mapping wasn't correct
            var testView = new TestView();
        }

        [TestMethod]
        public void Mediator_Works_On_Interface_Mapping()
        {
            _mapper.Map<ITestView>().ToMediator<InterfaceTestMediator>();

            var testView = new TestView();

            var dispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();

            bool listenerInvoked = false;
            dispatcher.AddListener<TestEvent>(TestEvent.Type.Test2, (evt) =>
            {
                listenerInvoked = true;
            });


            //Mediator has a listener on view for test 1, and dispatches test 2 when heard
            testView.Dispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            Assert.IsTrue(listenerInvoked);
        }

        [TestMethod]
        public void Mapper_Creates_Unique_Mediator_Per_View()
        {
            var dispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();

            _mapper.Map<TestView>().ToMediator<TestMediator>();

            int numberOfEventInvokes = 0;
            dispatcher.AddListener<TestEvent>(TestEvent.Type.Test2, (evt) =>
            {
                numberOfEventInvokes++;
            });


            int numberOfViews = 2;
            for(int i = 0; i < numberOfViews; i++)
            {
                //Mediator has a listener on view for test 1 event, and dispatches test 2 event when heard
                new TestView().Dispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));
            }

            Assert.IsTrue(numberOfEventInvokes == numberOfViews);
        }

        [TestMethod]
        public void Mapper_Creates_All_Mapped_Mediators_To_View()
        {
            var dispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();

            int expectedNumberOfInvokes = 2;
            _mapper.Map<TestView>().ToMediator<TestMediator>();
            _mapper.Map<TestView>().ToMediator<InterfaceTestMediator>();

            int numberOfEventInvokes = 0;
            dispatcher.AddListener<TestEvent>(TestEvent.Type.Test2, (evt) =>
            {
                numberOfEventInvokes++;
            });

            var view = new TestView();
            view.Dispatcher.Dispatch(new TestEvent(TestEvent.Type.Test1));

            //We would expect these to be equal, if both mapped mediators were created
            Assert.AreEqual(expectedNumberOfInvokes, numberOfEventInvokes);
        }
    }
}
