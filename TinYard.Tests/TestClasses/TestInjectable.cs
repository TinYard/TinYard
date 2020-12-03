using TinYard.Framework.Impl.Attributes;

namespace TinYard_Tests.TestClasses
{
    public class TestInjectable
    {
        [Inject]
        public int Value;

        [Inject("TestIn")]
        public string NamedInjectable;

        public float ConstructedFloat { get; private set; }

        public TestInjectable()
        {

        }

        public TestInjectable(float injectableFloat)
        {
            ConstructedFloat = injectableFloat;
        }
    }
}
