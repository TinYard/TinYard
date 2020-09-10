using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem;

namespace TinYard.Extensions.CommandSystem.Tests
{
    [TestClass]
    public class CommandSystemExtensionTests
    {
        private CommandSystemExtension _extension;
        private IContext _context;

        [TestInitialize]
        public void Setup()
        {
            _extension = new CommandSystemExtension();
            _context = new Context();
            _context.Install(new EventSystemExtension());
        }

        [TestCleanup]
        public void Teardown()
        {
            _extension = null;
        }

        [TestMethod]
        public void CommandSystemExtension_Is_IExtension()
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
        public void CommandMap_Is_Mapped()
        {
            _context.Install(_extension);
            _context.Initialize();

            Assert.IsNotNull(_context.Mapper.GetMappingValue<ICommandMap>());
        }
    }
}
