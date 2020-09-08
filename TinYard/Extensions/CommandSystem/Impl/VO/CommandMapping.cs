using System;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.VO
{
    public class CommandMapping : ICommandMapping
    {
        public Type Event { get; private set; }

        public Enum EventType { get; private set; }

        public Type Command { get; private set; }

        public ICommandMapping Map<T>(Enum type = null) where T : IEvent
        {
            Event = typeof(T);
            EventType = type;

            return this;
        }

        public ICommandMapping ToCommand<T>() where T : ICommand
        {
            Command = typeof(T);

            return this;
        }
    }
}
