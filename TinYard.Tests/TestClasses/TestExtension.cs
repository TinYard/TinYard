using TinYard.API.Interfaces;

namespace TinYard.Tests.MockClasses
{
    public class TestExtension : IExtension
    {
        public IContext context;

        public object Environment { get { return _environment; } }
        private object _environment;

        public void Install(IContext context)
        {
            this.context = context;
        }
    }
}
