using TinYard.API.Interfaces;

namespace TinYard_Tests.TestClasses
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
