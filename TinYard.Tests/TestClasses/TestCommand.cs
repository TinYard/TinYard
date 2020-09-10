using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Framework.Impl.Attributes;
using TinYard_Tests.TestClasses;

namespace TinYard.Tests.TestClasses
{
    public class TestCommand : ICommand
    {
        public static bool ExecuteCalled { get; set; } = false;

        [Inject]
        public TestEvent Evt;

        [Inject]
        public TestInjectable Injectable;

        public void Execute()
        {
            ExecuteCalled = true;
        }
    }
}
