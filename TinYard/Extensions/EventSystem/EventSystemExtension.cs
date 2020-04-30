using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;

namespace TinYard.Extensions.EventSystem
{
    public class EventSystemExtension : IExtension
    {
        private IContext _context;

        private IEventDispatcher _eventDispatcher;

        public void Install(IContext context)
        {
            _context = context;
            _eventDispatcher = new EventDispatcher(context);

            _context.Mapper.Map<IEventDispatcher>().ToValue(_eventDispatcher);
        }
    }
}
