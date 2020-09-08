using System;
using System.Collections.Generic;
using System.Text;
using TinYard.Extensions.CommandSystem.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.Factories
{
    public class CommandFactory : ICommandFactory
    {
        public ICommand Build<T>() where T : ICommand
        {
            return Build(typeof(T));
        }

        public ICommand Build(Type commandType)
        {
            if (!typeof(ICommand).IsAssignableFrom(commandType))
                return null;

            return Activator.CreateInstance(commandType) as ICommand;
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
