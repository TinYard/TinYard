using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.CommandMaps;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.CommandSystem
{
    public class CommandSystemExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;

            SetupCommandMap();
        }

        private void SetupCommandMap()
        {
            var eventDispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();
            var injector = _context.Injector;

            EventCommandMap commandMap = new EventCommandMap(eventDispatcher, injector);
            _context.Mapper.Map<ICommandMap>().ToValue(commandMap);
        }
    }
}
