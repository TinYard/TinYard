using System;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.EventSystem.Impl.Base
{
    public class Event : IEvent
    {
        public Enum type { get { return _type; } }
        private readonly Enum _type;

        public Event(Enum type)
        {
            _type = type;
        }
    }
}
