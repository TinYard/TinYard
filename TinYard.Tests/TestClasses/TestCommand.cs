using TinYard.Extensions.CommandSystem.API.Interfaces;

namespace TinYard.Tests.TestClasses
{
    public class TestCommand : ICommand
    {
        public bool ExecuteCalled { get; set; } = false;

        public void Execute()
        {
            ExecuteCalled = true;
        }
    }
}
