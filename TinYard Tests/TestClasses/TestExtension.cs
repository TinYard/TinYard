using TinYard.API.Interfaces;

namespace TinYard.Tests.MockClasses
{
    public class TestExtension : IExtension
    {
        public IContext context;

        public void Install(IContext context)
        {
            this.context = context;
        }
    }
}
