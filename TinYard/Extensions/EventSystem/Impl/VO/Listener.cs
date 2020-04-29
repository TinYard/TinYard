using System;
using System.Collections.Generic;

namespace TinYard.Extensions.EventSystem.Impl.VO
{
    public class Listener
    {
        public Enum @Type { get; private set; }
        public List<Delegate> ListenerCallbacks { get; private set; }

        public Listener()
        {
            ListenerCallbacks = new List<Delegate>();
        }

        public Listener(Enum type, Delegate listenerCallback)
        {
            Type = type;
            ListenerCallbacks = new List<Delegate>();
            ListenerCallbacks.Add(listenerCallback);
        }

        public void AddListener(Delegate listenerCallback)
        {
            ListenerCallbacks.Add(listenerCallback);
        }

        public void RemoveListener(Delegate listenerCallback)
        {
            ListenerCallbacks.Remove(listenerCallback);
        }
    }
}
