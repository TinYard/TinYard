using Microsoft.VisualStudio.TestTools.UnitTesting;
using TinYard.Framework.Impl.Attributes;
using TinYard_Tests.TestClasses;

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
            var fields = InjectAttribute.GetInjectableFields(typeof(TestInjectable));

            Assert.IsTrue(fields.Count > 0);
        }

        [TestMethod]
        public void Inject_Attribute_Provides_Constructors_To_Inject_Into()
        {
            var constructors = InjectAttribute.GetInjectableConstructors(typeof(TestSecondaryInjectable));

            Assert.IsTrue(constructors.Count > 0);
        }
    }
}
