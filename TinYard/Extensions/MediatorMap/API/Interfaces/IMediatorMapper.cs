using System;
using System.Collections.Generic;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Interfaces
{
    public interface IMediatorMapper
    {
        event Action<IMediatorMappingObject> OnMediatorMapping;
        IMediatorFactory MappingFactory { get; }

        IMediatorMappingObject Map<T>() where T : IView;
        IMediatorMappingObject Map(IView view);

        IMediatorMappingObject GetMapping<T>() where T : IView;
        IMediatorMappingObject GetMapping(IView type);
        IReadOnlyList<IMediatorMappingObject> GetAllMappings();

        object GetMappingMediator<T>() where T : IView;
        object GetMappingMediator(IView type);
    }
}
