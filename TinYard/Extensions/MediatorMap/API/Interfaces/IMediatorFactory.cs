using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Interfaces
{
    public interface IMediatorFactory : IFactory
    {
        IMediator Build(Type mediatorType);
        T Build<T>() where T : IMediator;
    }
}
