using TinYard.Framework.Impl.Attributes;

namespace TinYard_Tests.TestClasses
{
    public class TestSecondaryInjectable
    {
        [Inject]
        public TestInjectable InjectedValue;

        public float InjectableFloat { get; private set; }
        public double InjectableDouble { get; private set; }

        public TestSecondaryInjectable()
        {

        }

        public TestSecondaryInjectable(float injectableFloat, double injectableDouble)
        {
            InjectableFloat = injectableFloat;

            //Don't set the double, we want a constructor which has more parameters so we can make sure its choosing the attributed one
        }

        [Inject]
        public TestSecondaryInjectable(double injectableDouble)
        {
            InjectableDouble = injectableDouble;
        }
    }
}
