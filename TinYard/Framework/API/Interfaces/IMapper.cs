using System;
using System.Collections.Generic;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.VO;

namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        event Action<IMappingObject> OnValueMapped;
        IMappingFactory MappingFactory { get; }

        IMappingObject Map<T>();

        IMappingObject GetMapping<T>();
        IMappingObject GetMapping(Type type);
        IReadOnlyList<IMappingObject> GetAllMappings();

        object GetMappingValue<T>();
        object GetMappingValue(Type type);
    }
}
