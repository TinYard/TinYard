using TinYard.API.Interfaces;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;

            _context.Configure(new MediatorMapConfig());
        }
    }
}