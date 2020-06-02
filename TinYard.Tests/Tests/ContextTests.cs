using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.Exceptions;
using TinYard.Tests.MockClasses;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class ContextTests
    {
        private IContext _context;
        private TestExtension _testExtension;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
        }

        [TestMethod]
        public void Context_Is_IContext()
        {
            Assert.IsInstanceOfType(_context, typeof(IContext));
        }

        //
        //  Extensions and Configs
        //

        [TestMethod]
        public void Context_Installs_Extension()
        {
            _testExtension = new TestExtension();
            _context.Install(_testExtension);
            _context.Initialize();
            //Assert nothing, if it doesn't throw it's a success
        }

        [TestMethod]
        public void Context_Throws_On_Same_Extension_Multiple_Times()
        {
            _testExtension = new TestExtension();

            Assert.ThrowsException<ContextException>(() =>
            {
                _context.Install(_testExtension);
                _context.Install(_testExtension);

                _context.Initialize();
            });
        }

        [TestMethod]
        public void Context_Passes_Itself_To_Extension_On_Install()
        {
            _testExtension = new TestExtension();
            _context.Install(_testExtension);
            _context.Initialize();

            Assert.AreEqual(_context, _testExtension.context);
        }

        [TestMethod]
        public void Context_Configures_Extension()
        {
            _testExtension = new TestExtension();
            _context.Install(_testExtension).Configure(new TestConfig());
            _context.Initialize();
            //Assert nothing, if it doesn't throw it's a success
        }

        //
        //  Bundles
        //

        [TestMethod]
        public void Context_Installs_Bundle()
        {
            IBundle testBundle = new TestBundle();
            _context.Install(testBundle);
            _context.Initialize();

            //Assert nothing, if it doesn't throw it's a success
        }

        //
        //  Initialisations
        //

        [TestMethod]
        public void Context_Initializes_With_No_Errors()
        {
            //Assert nothing, if it doesn't throw it's a success
            _context.Initialize();
        }

        [TestMethod]
        public void Context_Throws_On_Multiple_Initializations()
        {
            Assert.ThrowsException<ContextException>(() =>
            {
                _context.Initialize();
                _context.Initialize();
            });
        }

        //
        //  Context Members
        //

        [TestMethod]
        public void Context_Has_Mapper()
        {
            Assert.IsNotNull(_context.Mapper);
        }

        [TestMethod]
        public void Context_Has_Injector()
        {
            Assert.IsNotNull(_context.Injector);
        }

        [TestMethod]
        public void Context_Provides_Config_Injections()
        {
            _testExtension = new TestExtension();
            TestConfig config = new TestConfig();
            _context.Install(_testExtension).Configure(config);
            _context.Initialize();

            Assert.AreEqual(_context, config.context);
        }

        [TestMethod]
        public void Context_Injects_Values_Mapped()
        {
            _context.Initialize();

            TestInjectable testInjectable = new TestInjectable();

            int testValue = 5;
            int preInjectValue = testInjectable.Value;

            Assert.IsTrue(preInjectValue != testValue);

            //Map an int value so that we can test if it's changed later
            _context.Mapper.Map<int>().ToValue(testValue);

            //Injector should run on this happening
            _context.Mapper.Map<TestInjectable>().ToValue(testInjectable);

            int postInjectValue = testInjectable.Value;

            Assert.IsTrue(preInjectValue != postInjectValue);
            Assert.AreEqual(testValue, postInjectValue);
        }
    }
}
