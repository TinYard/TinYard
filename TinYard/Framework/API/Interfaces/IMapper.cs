using System;
using System.Collections.Generic;
using TinYard.Impl.VO;

namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        event Action<IMappingObject> OnValueMapped;

        IMappingObject Map<T>(bool autoCreateValue = false);

        IMappingObject GetMapping<T>();
        IMappingObject GetMapping(Type type);
        IReadOnlyList<IMappingObject> GetAllMappings();

        object GetMappingValue<T>();
        object GetMappingValue(Type type);
    }
}
