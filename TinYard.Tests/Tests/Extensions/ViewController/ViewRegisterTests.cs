using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;

namespace TinYard.Extensions.ViewController.Tests
{
    [TestClass]
    public class ViewRegisterTests
    {
        private IContext _context;
        private IViewRegister _register;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _register = new ViewRegister(_context);
        }

        [TestCleanup]
        public void Teardown()
        {
            _register = null;
        }

        [TestMethod]
        public void ViewRegister_is_IViewRegister()
        {
            Assert.IsInstanceOfType(_register, typeof(IViewRegister));
        }
    }
}