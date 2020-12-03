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

        IMappingObject Map<T>(string mappingName = null);

        IMappingObject GetMapping<T>(string mappingName = null);

        IMappingObject GetMapping(Type type, string mappingName = null);
        IReadOnlyList<IMappingObject> GetAllMappings();

        T GetMappingValue<T>(string mappingName = null);

        object GetMappingValue(Type type, string mappingName = null);
    }
}
