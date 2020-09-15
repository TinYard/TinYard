using System;

namespace TinYard.Extensions.EventSystem.Impl.Exceptions
{
    public class EventTypeException : Exception
    {
        public EventTypeException() : base() { }

        public EventTypeException(string message) : base(message) { }

        public EventTypeException(string message, Exception innerException) : base(message, innerException) { }
    }
}
