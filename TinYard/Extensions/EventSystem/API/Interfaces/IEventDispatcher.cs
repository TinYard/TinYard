using System;

namespace TinYard.Extensions.EventSystem.API.Interfaces
{
    public interface IEventDispatcher : IDispatcher
    {
        void AddListener<T>(Enum type, Action<T> listenerCallback);
        void AddListener(Enum type, Action listenerCallback);
        void AddListener(Enum type, Delegate listenerCallback);

        void RemoveListener<T>(Enum type, Action<T> listenerCallback);
        void RemoveListener(Enum type, Action listenerCallback);
        void RemoveListener(Enum type, Delegate listenerCallback);

        void RemoveAllListeners(Enum type);
        void RemoveAllListeners();

        bool HasListener(Enum type);
    }
}
