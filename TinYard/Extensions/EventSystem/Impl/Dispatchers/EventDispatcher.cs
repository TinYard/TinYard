using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl.VO;

namespace TinYard.Extensions.EventSystem.Impl
{
    public class EventDispatcher : IEventDispatcher
    {
        private Dictionary<Enum, Listener> _listeners = new Dictionary<Enum, Listener>();

        private IContext _context;

        public EventDispatcher(IContext context = null)
        {
            _context = context;
        }

        public bool HasListener(Enum type)
        {
            return _listeners.ContainsKey(type);
        }

        public void AddListener<T>(Enum type, Action<T> listenerCallback)
        {
            AddListener(type, listenerCallback as Delegate);
        }

        public void AddListener(Enum type, Action listenerCallback)
        {
            AddListener(type, listenerCallback as Delegate);
        }

        public void AddListener(Enum type, Delegate listenerCallback)
        {
            if (HasListener(type))
            {
                _listeners[type].AddListener(listenerCallback);
            }
            else
            {
                _listeners.Add(type, new Listener(type, listenerCallback));
            }
        }

        public void RemoveListener(Enum type, Action listenerCallback)
        {
            RemoveListener(type, listenerCallback as Delegate);
        }

        public void RemoveListener<T>(Enum type, Action<T> listenerCallback)
        {
            RemoveListener(type, listenerCallback as Delegate);
        }

        public void RemoveListener(Enum type, Delegate listenerCallback)
        {
            if(HasListener(type))
            {
                Listener listener = _listeners[type];
                listener.RemoveListener(listenerCallback);

                if (listener.ListenerCallbacks.Count == 0)
                    _listeners.Remove(type);
            }
        }

        public void RemoveAllListeners(Enum type)
        {
            _listeners.Remove(type);
        }

        public void RemoveAllListeners()
        {
            _listeners.Clear();
        }

        public void Dispatch(IEvent evt)
        {
            if(HasListener(evt.type))
            {
                List<Delegate> callbacks = _listeners[evt.type].ListenerCallbacks;

                foreach(Delegate callback in callbacks)
                {
                    //Apparently Dynamic Invoke on Actions is expensive compared to on Delegates
                    //TODO : Do something about using delegates here instead
                    if(callback.Method.GetParameters().Length == 0)
                    {
                        callback.DynamicInvoke(null);
                    }
                    else
                    {
                        callback.DynamicInvoke(new object[] { evt });
                    }
                }
            }
        }
    }
}
