using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TinYard.API.Interfaces;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class ContextTest
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

            Assert.IsTrue(_context.ContainsExtension(_testExtension));
        }

        [TestMethod]
        public void Context_Initializes_With_No_Errors()
        {
            try
            {
                _context.Initialize();
                Assert.IsTrue(true);//If we make it here, no errors thrown
            }
            catch (Exception)
            {
                Assert.Fail();
            }
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
            try
            {
                _context.Initialize();
                _context.Initialize();
                Assert.Fail();//If we make it here, we failed and didn't error
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(Exception));
            }
        }
    }
}
