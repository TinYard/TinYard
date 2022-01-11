using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Injectors;
using TinYard.Tests.TestClasses;

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
            _injector = new TinYardInjector(_context.Mapper, null);
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

            TestInjectable constructed = _injector.CreateInjected<TestInjectable>();

            Assert.AreEqual(constructed.ConstructedFloat, expected);
        }

        [TestMethod]
        public void Injector_Prefers_Attributed_Constructor()
        {
            float expectedFloat = 3.14f;
            double expectedDouble = 3.147d;

            _context.Mapper.Map<float>().ToValue(expectedFloat);
            _context.Mapper.Map<double>().ToValue(expectedDouble);

            TestSecondaryInjectable constructed = _injector.CreateInjected<TestSecondaryInjectable>();

            Assert.AreNotEqual(constructed.InjectableFloat, expectedFloat);
            Assert.AreEqual(constructed.InjectableDouble, expectedDouble);
        }

        [TestMethod]
        public void Injector_Injects_Into_Named_Attributes()
        {
            string valueToInject = "foobar";
            _context.Mapper.Map<string>("TestIn").ToValue(valueToInject);

            TestInjectable injectable = new TestInjectable();

            string preInjectValue = injectable.NamedInjectable;
            _injector.Inject(injectable);

            //Shouldn't be the same as pre-inject
            Assert.AreNotEqual(preInjectValue, injectable.NamedInjectable);

            //Should be the value we mapped
            Assert.AreEqual(injectable.NamedInjectable, valueToInject);
        }

        [TestMethod]
        public void Injector_Can_Inject_Into_Property()
        {
            double expected = 69d;
            _context.Mapper.Map<double>().ToValue(expected);

            TestInjectable injectable = new TestInjectable();

            _injector.Inject(injectable);

            Assert.AreEqual(expected, injectable.InjectableProperty);
        }

        [TestMethod]
        public void Injector_Can_Inject_Into_Private_Set_Property()
        {
            double expected = 69d;
            _context.Mapper.Map<double>().ToValue(expected);

            TestInjectable injectable = new TestInjectable();

            _injector.Inject(injectable);

            Assert.AreEqual(expected, injectable.InjectablePrivateProperty);
        }

        [TestMethod]
        public void Injector_Can_Directly_Inject_Property()
        {
            double expected = 69d;

            TestInjectable injectable = new TestInjectable();

            _injector.Inject(injectable, expected);

            Assert.AreEqual(expected, injectable.InjectableProperty);
        }

        [TestMethod]
        public void Injector_Can_Directly_Inject_Private_Property()
        {
            double expected = 69d;

            TestInjectable injectable = new TestInjectable();

            _injector.Inject(injectable, expected);

            Assert.AreEqual(expected, injectable.InjectablePrivateProperty);
        }

        [TestMethod]
        public void Injector_Provides_Multiple_Injectables()
        {
            double valToInject1 = 3.14d;
            double valToInject2 = 7.28d;
            _context.Mapper.Map<double>().ToValue(valToInject1);
            _context.Mapper.Map<double>().ToValue(valToInject2);

            TestInjectable injectable = new TestInjectable();
            _injector.Inject(injectable);

            var injectedVals = injectable.MultipleInjectedDoubles;

            Assert.IsTrue(
                injectedVals.Contains(valToInject1) &&
                injectedVals.Contains(valToInject2)
                );
        }

        [TestMethod]
        public void Injector_Injects_Into_Multiple_Injectables_List()
        {
            double valToInject1 = 3.14d;
            double valToInject2 = 7.28d;
            _context.Mapper.Map<double>().ToValue(valToInject1);
            _context.Mapper.Map<double>().ToValue(valToInject2);

            int valToInject3 = 69;
            _context.Mapper.Map<int>().ToValue(valToInject3);

            TestInjectable valToInject4 = new TestInjectable();
            TestInjectable valToInject5 = new TestInjectable();
            _context.Mapper.Map<TestInjectable>().ToValue(valToInject4);
            _context.Mapper.Map<TestInjectable>().ToValue(valToInject5);

            TestTertiaryInjectable injectable = new TestTertiaryInjectable();
            _injector.Inject(injectable);

            var primaryInjectables = injectable.MultipleInjectables;

            foreach(TestInjectable primaryInjectable in primaryInjectables)
            {
                Assert.IsTrue(
                    primaryInjectable.MultipleInjectedDoubles.Contains(valToInject1) &&
                    primaryInjectable.MultipleInjectedDoubles.Contains(valToInject2)
                    );

                Assert.AreEqual(valToInject3, primaryInjectable.Value);
            }
        }

        [TestMethod]
        public void Injector_Only_Injects_Multiple_When_Requested()
        {
            double valToInject1 = 3.14d;
            double valToInject2 = 7.28d;
            _context.Mapper.Map<double>().ToValue(valToInject1);
            _context.Mapper.Map<double>().ToValue(valToInject2);

            int valToInject3 = 69;
            _context.Mapper.Map<int>().ToValue(valToInject3);

            List<TestInjectable> valToInject4 = new List<TestInjectable>();
            var mockVal1 = new TestInjectable();
            var mockVal2 = new TestInjectable();
            valToInject4.Add(mockVal1);
            valToInject4.Add(mockVal2);

            TestInjectable valToInject5 = new TestInjectable();
            TestInjectable valToInject6 = new TestInjectable();

            _context.Mapper.Map<IEnumerable<TestInjectable>>().ToValue(valToInject4);
            _context.Mapper.Map<TestInjectable>().ToValue(valToInject5);
            _context.Mapper.Map<TestInjectable>().ToValue(valToInject6);

            TestTertiaryInjectable injectable = new TestTertiaryInjectable();
            _injector.Inject(injectable);

            var primaryInjectables = injectable.NotMultipleInjectables;

            Assert.IsTrue(primaryInjectables.Contains(mockVal1));
            Assert.IsTrue(primaryInjectables.Contains(mockVal2));
        }
    }
}
