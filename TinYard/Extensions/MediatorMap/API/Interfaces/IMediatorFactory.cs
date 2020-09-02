using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Interfaces
{
    public interface IMediatorFactory : IFactory
    {
        IMediator Build(Type mediatorType);
        IMediator Build<T>() where T : IMediator;
    }
}
