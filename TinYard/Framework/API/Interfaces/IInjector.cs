using System;

namespace TinYard.Framework.API.Interfaces
{
    public interface IInjector
    {
        void AddInjectable(Type injectableType, object injectableObject);

        T Inject<T>();
        void Inject(object target);
        void Inject(object target, object value);
    }
}
