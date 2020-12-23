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

        object Environment { get; set; }

        IMappingObject Map<T>();
        IMappingObject Map<T>(string mappingName);

        IMappingObject GetMapping<T>();
        IMappingObject GetMapping<T>(string mappingName);

        IMappingObject GetMapping(Type type);
        IMappingObject GetMapping(Type type, string mappingName);

        IReadOnlyList<IMappingObject> GetAllMappings();
        IReadOnlyList<IMappingObject> GetAllNamedMappings();

        T GetMappingValue<T>();
        T GetMappingValue<T>(string mappingName);

        object GetMappingValue(Type type);
        object GetMappingValue(Type type, string mappingName);
    }
}
