using TinYard.Framework.API.Interfaces;

namespace TinYard.Framework.API.Base
{
    public abstract class Guard : IGuard
    {
        public Guard() 
        {
        }

        public abstract bool Satisfies();
    }
}
