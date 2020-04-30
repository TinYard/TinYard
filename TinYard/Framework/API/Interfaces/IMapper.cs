using System;
using TinYard.Impl.VO;

namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        IMappingObject Map<T>();

        IMappingObject GetMapping<T>();
        IMappingObject GetMapping(Type type);

        object GetMappingValue<T>();
        object GetMappingValue(Type type);
    }
}
