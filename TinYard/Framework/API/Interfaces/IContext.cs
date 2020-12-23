using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.API.Interfaces
{
    public interface IContext
    {
        //Timeline properties
        event Action PreExtensionsInstalled;
        event Action PostExtensionsInstalled;
        event Action PreConfigsInstalled;
        event Action PostConfigsInstalled;
        event Action PostInitialize;

        //Properties
        IMapper Mapper { get; }
        IInjector Injector { get; }

        object Environment { get; set; }

        IContext Install(IExtension extension);
        IContext Install(IBundle bundle);
        IContext Configure(IConfig configuration);

        void Initialize();

        bool ContainsExtension(IExtension extension);
        bool ContainsExtension<T>() where T : IExtension;

        void Detain(object objToDetain);
        void Release(object objToRelease);
    }
}
