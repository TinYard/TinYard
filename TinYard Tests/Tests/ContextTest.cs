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
    }
}
