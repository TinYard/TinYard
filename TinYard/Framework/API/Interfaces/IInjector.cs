using System;

namespace TinYard.Framework.API.Interfaces
{
    public interface IInjector
    {
        void AddInjectable(Type injectableType, object injectableObject);

        void Inject(object classToInjectInto);
    }
}
