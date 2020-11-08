using System;
using TinYard.Extensions.EventSystem.Impl.Base;

namespace TinYard.Extensions.CallbackTimer.API.Events
{
    public class RemoveCallbackTimerEvent : Event
    {
        public enum Type
        {
            Remove
        }

        public Action CallbackToRemove { get; private set; }

        public RemoveCallbackTimerEvent(Type type, Action callbackToRemove) : base(type)
        {
            CallbackToRemove = callbackToRemove;
        }
    }
}
