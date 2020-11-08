using TinYard.Extensions.CommandSystem.API.Interfaces;

namespace TinYard.Tests.TestClasses
{
    public class TestGuardedCommand : ICommand
    {
        public static bool ExecuteCalled = false;

        public void Execute()
        {
            ExecuteCalled = true;
        }
    }
}
