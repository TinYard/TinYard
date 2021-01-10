using TinYard.Framework.API.Base;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestGuard : Guard
    {
        [Inject]
        public string Injectable;

        public override bool Satisfies()
        {
            return Injectable != null;
        }
    }
}
