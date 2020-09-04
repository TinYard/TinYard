using TinYard.API.Interfaces;

namespace TinYard.Extensions.Logging
{
    public class LoggingExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;
        }
    }
}
