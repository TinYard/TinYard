using System.Collections.Generic;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestTertiaryInjectable
    {
        [Inject(allowMultiple: true)]
        public IEnumerable<TestInjectable> MultipleInjectables;

        [Inject]
        public IEnumerable<TestInjectable> NotMultipleInjectables;
    }
}
