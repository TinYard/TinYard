using System;
using System.Collections.Generic;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.Factories
{
    public class CommandFactory : ICommandFactory
    {
        private IInjector _injector;

        public CommandFactory(IInjector injector)
        {
            _injector = injector;
        }

        public T Build<T>(IEvent evtToInject = null) where T : ICommand
        {
            return (T)Build(typeof(T), evtToInject);
        }

        public ICommand Build(Type commandType, IEvent evtToInject = null)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
                return null;

            var command = Activator.CreateInstance(commandType) as ICommand;

            if (command == null)
                return null;

            //Perform injections
            _injector.Inject(command);
            if (evtToInject != null)
                _injector.Inject(command, evtToInject);

            return command;
        }

        public object Build(params object[] args)
        {
            List<ICommand> builtCommands = new List<ICommand>();
            foreach (object arg in args)
            {
                var command = Build(arg.GetType());

                if (command != null)
                    builtCommands.Add(command);
            }

            return builtCommands;
        }
    }
}
