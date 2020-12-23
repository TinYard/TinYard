using System;

namespace TinYard.Framework.API.Interfaces
{
    public interface IInjector
    {
        object Environment { get; set; }

        void AddInjectable(Type injectableType, object injectableObject);

        T CreateInjected<T>();
        object CreateInjected(Type targetType);

        void Inject(object target);
        void Inject(object target, object value);
    }
}
