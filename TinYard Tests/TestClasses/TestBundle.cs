using TinYard.API.Interfaces;

namespace TinYard_Tests.TestClasses
{
    public class TestBundle : IBundle
    {
        public void Install(IContext context)
        {
            context
                .Install(new TestExtension())
                .Configure(new TestConfig());
        }
    }
}
