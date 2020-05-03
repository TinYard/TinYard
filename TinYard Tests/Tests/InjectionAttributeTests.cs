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
            object[] fields = InjectAttribute.GetInjectables(typeof(TestInjectable)).ToArray();

            Assert.IsTrue(fields.Length > 0);
        }
    }
}
