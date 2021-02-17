using TinYard.API.Interfaces;

namespace TinYard.Extensions.Logging
{
    public class LoggingExtension : IExtension
    {
        public object Environment { get { return _environment; } }
        private object _environment;

        private IContext _context;

        public LoggingExtension(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            _context = context;
        }
    }
}
