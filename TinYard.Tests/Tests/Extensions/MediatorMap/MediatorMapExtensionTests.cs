using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Tests
{
    [TestClass]
    public class MediatorMapExtensionTests
    {
        private IContext _context;
        private MediatorMapExtension _extension;

        [TestInitialize]
        public void Setup()
        {
            _context = new Context();
            _extension = new MediatorMapExtension();
        }

        [TestCleanup]
        public void Teardown()
        {
            _context = null;
            _extension = null;
        }

        [TestMethod]
        public void MediatorMapExtension_Is_IExtension()
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
        public void Extension_Maps_MediatorMapper()
        {
            SetupExtension();

            Assert.IsNotNull(_context.Mapper.GetMappingValue<IMediatorMapper>());
        }

        private void SetupExtension()
        {
            _context.Install(_extension);
            _context.Initialize();
        }
    }
}
