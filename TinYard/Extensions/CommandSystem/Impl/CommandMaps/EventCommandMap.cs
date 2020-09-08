using System;
using System.Collections.Generic;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.CommandSystem.Impl.Factories;
using TinYard.Extensions.CommandSystem.Impl.VO;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.CommandMaps
{
    public class EventCommandMap : ICommandMap
    {
        private IEventDispatcher _eventDispatcher;
        private IInjector _injector;

        private ICommandFactory _commandFactory;

        private List<ICommandMapping> _mappings = new List<ICommandMapping>();

        public EventCommandMap(IEventDispatcher eventDispatcher, IInjector injector)
        {
            _eventDispatcher = eventDispatcher;
            _injector = injector;

            _commandFactory = new CommandFactory(injector);
        }

        public ICommandMapping Map<T>() where T : IEvent
        {
            var mapping = AddListener<T>();
            
            _mappings.Add(mapping);

            return mapping;
        }

        public ICommandMapping Map<T>(Enum type) where T : IEvent
        {
            var mapping = AddListener<T>(type);

            _mappings.Add(mapping);

            return mapping;
        }

        private ICommandMapping AddListener<T>(Enum eventType = null) where T : IEvent
        {
            ICommandMapping mapping = new CommandMapping();
            mapping = mapping.Map<T>(eventType);

            _eventDispatcher.AddListener<T>(eventType, (evt)=> ExecuteMapping(mapping, evt));

            return mapping;
        }

        private void ExecuteMapping(ICommandMapping mapping, IEvent evt)
        {
            if (mapping.Command == null)
                return;

            //Should be injected into in the Factory
            ICommand builtCommand = _commandFactory.Build(mapping.Command, evt);
            builtCommand.Execute();//Let the command do its thing
        }
    }
}
