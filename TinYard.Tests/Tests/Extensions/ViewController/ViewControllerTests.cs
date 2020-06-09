using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TinYard.Extensions.ViewController.Tests
{
    [TestClass]
    public class ViewControllerTests
    {
        private IViewController _controller;

        [TestInitialize]
        public void Setup()
        {
            _controller = new ViewController();
        }

        [TestCleanup]
        public void Teardown()
        {
            _controller = null;
        }

        [TestMethod]
        public void ViewController_Is_IViewController()
        {
            Assert.IsInstanceOfType(_controller, typeof(IViewController));
        }
    }
}