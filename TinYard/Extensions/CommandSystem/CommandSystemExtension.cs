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
            _context.PostConfigsInstalled += PostConfigsInstalled;
        }

        private void PostConfigsInstalled()
        {
            var eventDispatcher = _context.Mapper.GetMappingValue<IEventDispatcher>();
            var injector = _context.Injector;

            if(eventDispatcher == null || injector == null)
            {
                throw new NullReferenceException("Event Dispatcher or Injector are null in CommandSystemExtension");
            }

            EventCommandMap commandMap = new EventCommandMap(eventDispatcher, injector);
            _context.Mapper.Map<ICommandMap>().ToValue(commandMap);
        }
    }
}
