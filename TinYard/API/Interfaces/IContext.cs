using System;

namespace TinYard.API.Interfaces
{
    public interface IContext
    {
        event Action PreExtensionsInstalled;
        event Action PostExtensionsInstalled;
        event Action PreConfigsInstalled;
        event Action PostConfigsInstalled;

        IContext Install(IExtension extension);
        IContext Install(IBundle bundle);
        IContext Configure(IConfig configuration);

        void Initialize();

        bool ContainsExtension(IExtension extension);
        bool ContainsExtension<T>() where T : IExtension;
    }
}
