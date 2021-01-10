using TinYard.API.Interfaces;

namespace TinYard.Tests.MockClasses
{
    public class TestBundle : IBundle
    {
        public object Environment { get { return _environment; } }
        private object _environment;
        
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
