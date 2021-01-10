using TinYard.API.Interfaces;

namespace TinYard.Tests.MockClasses
{
    public class TestBundle : IBundle
    {
        public readonly bool HaveDependencies;

        public TestBundle(bool haveDependencies = false)
        {
            HaveDependencies = haveDependencies;
        }

        public void Install(IContext context)
        {
            context
                .Install(new TestExtension())
                .Configure(new TestConfig(HaveDependencies));
        }
    }
}
