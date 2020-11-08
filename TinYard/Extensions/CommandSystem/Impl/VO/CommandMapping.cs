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

        public ICommandMapping WithGuard<T1, T2>() where T1 : Guard where T2 : Guard
        {
            _guardTypes.Add(typeof(T1));
            _guardTypes.Add(typeof(T2));

            return this;
        }

        public ICommandMapping WithGuard<T1, T2, T3>() where T1 : Guard where T2 : Guard where T3 : Guard
        {
            _guardTypes.Add(typeof(T1));
            _guardTypes.Add(typeof(T2));
            _guardTypes.Add(typeof(T3));

            return this;
        }

        public ICommandMapping WithGuard<T1, T2, T3, T4>() where T1 : Guard where T2 : Guard where T3 : Guard where T4 : Guard
        {
            _guardTypes.Add(typeof(T1));
            _guardTypes.Add(typeof(T2));
            _guardTypes.Add(typeof(T3));
            _guardTypes.Add(typeof(T4));

            return this;
        }

        public ICommandMapping WithGuard<T1, T2, T3, T4, T5>() where T1 : Guard where T2 : Guard where T3 : Guard where T4 : Guard where T5 : Guard
        {
            _guardTypes.Add(typeof(T1));
            _guardTypes.Add(typeof(T2));
            _guardTypes.Add(typeof(T3));
            _guardTypes.Add(typeof(T4));
            _guardTypes.Add(typeof(T5));

            return this;
        }
    }
}
