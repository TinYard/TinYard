using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;

namespace TinYard.Extensions.EventSystem
{
    public class EventSystemExtension : IExtension
    {
        public object Environment { get { return _environment; } }
        private object _environment;

        private IContext _context;

        private IEventDispatcher _eventDispatcher;

        public EventSystemExtension(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            _context = context;
            _eventDispatcher = new EventDispatcher(context);

            _context.Mapper.Map<IEventDispatcher>().ToSingleton(_eventDispatcher);
        }
    }
}
