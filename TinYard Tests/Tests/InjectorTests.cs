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
    }
}
