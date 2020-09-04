using System;
using System.Collections.Generic;
using TinYard.Extensions.MediatorMap.API.VO;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Interfaces
{
    public interface IMediatorMapper
    {
        event Action<IMediatorMappingObject> OnMediatorMapping;
        IMediatorFactory MediatorFactory { get; }

        IMediatorMappingObject Map<T>() where T : IView;
        IMediatorMappingObject Map(IView view);

        IMediatorMappingObject GetMapping<T>() where T : IView;
        IMediatorMappingObject GetMapping(IView type);
        IReadOnlyList<IMediatorMappingObject> GetAllMappings();

        object GetMappingMediator<T>() where T : IView;
        object GetMappingMediator(IView type);
    }
}
