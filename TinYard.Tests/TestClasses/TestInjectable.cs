using System.Collections;
using System.Collections.Generic;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestInjectable
    {
        [Inject]
        public int Value;

        [Inject("TestIn")]
        public string NamedInjectable;

        [Inject(allowMultiple: true)]
        public IEnumerable<double> MultipleInjectedDoubles;

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
