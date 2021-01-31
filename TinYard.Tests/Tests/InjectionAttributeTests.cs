using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TinYard.Framework.Impl.Attributes;
using TinYard.Tests.TestClasses;

namespace TinYard.Tests
{
    [TestClass]
    public class InjectionAttributeTests
    {
        [TestInitialize]
        public void Setup()
        {
        }

        [TestCleanup]
        public void Teardown()
        {
        }

        [TestMethod]
        public void Inject_Attribute_Provides_Fields_To_Inject_Into()
        {
            var infos = InjectAttribute.GetInjectableInformation(typeof(TestInjectable));

            Assert.IsTrue(infos.Count > 0);
            Assert.IsTrue(infos.Any(info => info.Field != null));
        }

        [TestMethod]
        public void Inject_Attribute_Provides_Properties_To_Inject_Into()
        {
            var infos = InjectAttribute.GetInjectableInformation(typeof(TestInjectable));

            Assert.IsTrue(infos.Count > 0);
            Assert.IsTrue(infos.Any(info => info.Property != null));
        }

        [TestMethod]
        public void Inject_Attribute_Provides_Constructors_To_Inject_Into()
        {
            var constructors = InjectAttribute.GetInjectableConstructors(typeof(TestSecondaryInjectable));

            Assert.IsTrue(constructors.Count > 0);
        }
    }
}
