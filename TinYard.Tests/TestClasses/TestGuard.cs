using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestGuard : IGuard
    {
        [Inject]
        public object Injectable;

        public bool Satisfies()
        {
            return Injectable != null;
        }
    }
}
