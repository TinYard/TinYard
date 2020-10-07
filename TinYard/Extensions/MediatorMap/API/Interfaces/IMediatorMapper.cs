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

        IMediatorMappingObject Map<T>();
        IMediatorMappingObject Map(IView view);
        IMediatorMappingObject Map(object view);

        IEnumerable<IMediatorMappingObject> GetMappings<T>();
        IEnumerable<IMediatorMappingObject> GetMappings(IView type);
        IReadOnlyList<IMediatorMappingObject> GetAllMappings();
    }
}
