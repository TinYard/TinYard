using TinYard.Framework.Impl.Attributes;

namespace TinYard_Tests.TestClasses
{
    public class TestSecondaryInjectable
    {
        [Inject]
        public TestInjectable InjectedValue;
    }
}
