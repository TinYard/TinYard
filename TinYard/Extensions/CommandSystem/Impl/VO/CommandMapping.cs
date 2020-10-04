using System;
using System.Collections.Generic;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Base;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.Impl.VO
{
    public class CommandMapping : ICommandMapping
    {
        public Type Event { get; private set; }

        public Enum EventType { get; private set; }

        public Type Command { get; private set; }

        public IReadOnlyList<Type> GuardTypes { get { return _guardTypes.AsReadOnly(); } }

        private List<Type> _guardTypes = new List<Type>();

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

        public ICommandMapping WithGuard<T>() where T : Guard
        {
            _guardTypes.Add(typeof(T));

            return this;
        }
    }
}
