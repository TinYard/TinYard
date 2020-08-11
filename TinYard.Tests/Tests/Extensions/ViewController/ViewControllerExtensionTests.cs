using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.ViewController.Tests
{
    [TestClass]
    public class ViewControllerExtensionTests
    {
        private IContext _context;
        private ViewControllerExtension _extension;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _extension = new ViewControllerExtension();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _extension = null;
        }

        [TestMethod]
        public void ViewControllerExtension_Is_IExtension()
        {
            Assert.IsInstanceOfType(_extension, typeof(IExtension));
        }

        [TestMethod]
        public void Context_Installs_Extension()
        {
            _context.Install(_extension);
            _context.Initialize();
        }

        [TestMethod]
        public void ViewRegister_Is_Mapped()
        {
            _context.Install(_extension);
            _context.Initialize();

            Assert.IsNotNull(_context.Mapper.GetMappingValue<IViewRegister>());
        }
    }
}