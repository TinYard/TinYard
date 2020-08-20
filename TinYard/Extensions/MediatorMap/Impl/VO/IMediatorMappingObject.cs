using System;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.VO
{
    public interface IMediatorMappingObject
    {
        Type View { get; }
        Type Mediator { get; }

        event Action<IMediatorMappingObject> OnViewMapped;

        IMediatorMappingObject Map<T>();

        IMediatorMappingObject ToMediator<T>() where T : IMediator;
        IMediatorMappingObject ToMediator(IMediator mediator);
    }
}
