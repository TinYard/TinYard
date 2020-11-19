using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Injectors;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class InjectorTests
    {
        private IContext _context;
        private IInjector _injector;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _injector = new TinYardInjector(_context);
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _injector = null;
        }

        [TestMethod]
        public void Injector_is_IInjector()
        {
            Assert.IsInstanceOfType(_injector, typeof(IInjector));
        }

        [TestMethod]
        public void Injector_injects_into_class()
        {
            int valueToInject = 5;
            _context.Mapper.Map<int>().ToValue(valueToInject);

            TestInjectable injectable = new TestInjectable();

            int preInjectValue = injectable.Value;
            _injector.Inject(injectable);

            //Shouldn't be 0 (default value)
            Assert.AreNotEqual(preInjectValue, injectable.Value);

            //Should be the value we mapped (5)
            Assert.AreEqual(injectable.Value, valueToInject);
        }

        [TestMethod]
        public void Injector_Injects_From_Extra_Injectables()
        {
            int valueToInject = 5;

            _injector.AddInjectable(valueToInject.GetType(), valueToInject);

            TestInjectable injectable = new TestInjectable();
            int preInjectValue = injectable.Value;

            Assert.AreNotEqual(preInjectValue, valueToInject);

            _injector.Inject(injectable);

            Assert.AreEqual(valueToInject, injectable.Value);
        }

        [TestMethod]
        public void Injector_Directly_Injects_Value()
        {
            int expected = 5;

            TestInjectable injectable = new TestInjectable();
            int preInjectValue = injectable.Value;

            Assert.AreNotEqual(preInjectValue, expected);

            _injector.Inject(injectable, expected);

            Assert.AreEqual(expected, injectable.Value);
        }

        [TestMethod]
        public void Injector_Injects_Into_Injectable_Values()
        {
            TestInjectable injectable = new TestInjectable();
            _context.Mapper.Map<TestInjectable>().ToValue(injectable);

            //Making sure it's not been set somehow by accident
            Assert.AreEqual(default(int), injectable.Value);

            //Map an int so that when we call `Inject` on a class that needs a `TestInjectable`, 
            //it can Inject this int into the `TestInjectable` value
            int expected = 5;
            _context.Mapper.Map<int>().ToValue(expected);

            _injector.Inject(new TestSecondaryInjectable());

            Assert.AreEqual(expected, injectable.Value);
        }

        [TestMethod]
        public void Injector_Can_Create_Injectable_Constructor()
        {
            float expected = 3.14f;
            _context.Mapper.Map<float>().ToValue(expected);

            TestInjectable constructed = _injector.Inject<TestInjectable>();

            Assert.AreEqual(constructed.ConstructedFloat, expected);
        }
    }
}
