using TinYard.API.Interfaces;

namespace TinYard.Tests.TestClasses
{
    public class TestCreatable
    {
        public IContext Context { get; }

        public TestCreatable(IContext context)
        {
            Context = context;
        }
    }
}
