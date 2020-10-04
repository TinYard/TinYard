using System;
using System.Collections.Generic;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.VO
{
    public class CommandMapping : ICommandMapping
    {
        public Type Event { get; private set; }

        public Enum EventType { get; private set; }

        public Type Command { get; private set; }

        public IReadOnlyList<IGuard> Guards { get { return _guards.AsReadOnly(); } }

        private List<IGuard> _guards = new List<IGuard>();

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

        public ICommandMapping WithGuard<T>() where T : IGuard
        {
            throw new NotImplementedException();
        }
    }
}
