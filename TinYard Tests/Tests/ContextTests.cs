using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Impl.Exceptions;
using TinYard.Tests.MockClasses;

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

        [TestMethod]
        public void Context_Installs_Extension()
        {
            _testExtension = new TestExtension();
            _context.Install(_testExtension);
            _context.Initialize();
            //Assert nothing, if it doesn't throw it's a success
        }

        [TestMethod]
        public void Context_Configures_Extension()
        {
            _testExtension = new TestExtension();
            _context.Install(_testExtension).Configure(new TestConfig());
            _context.Initialize();
            //Assert nothing, if it doesn't throw it's a success
        }

        [TestMethod]
        public void Context_Installs_Bundle()
        {
            IBundle testBundle = new TestBundle();
            _context.Install(testBundle);
            _context.Initialize();

            //Assert nothing, if it doesn't throw it's a success
        }

        [TestMethod]
        public void Context_Initializes_With_No_Errors()
        {
            //Assert nothing, if it doesn't throw it's a success
            _context.Initialize();
            Assert.IsTrue(true);
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
        public void Context_Throws_On_Multiple_Initializations()
        {
            Assert.ThrowsException<ContextException>(() =>
            {
                _context.Initialize();
                _context.Initialize();
            });
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
        public void Context_Has_Mapper()
        {
            Assert.IsNotNull(_context.Mapper);
        }
    }
}
