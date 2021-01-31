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
        IMappingObject Map<T>(object environment);
        IMappingObject Map<T>(string mappingName);
        IMappingObject Map<T>(object environment, string mappingName);

        IMappingObject GetMapping<T>();
        IMappingObject GetMapping<T>(object environment);
        IMappingObject GetMapping<T>(string mappingName);
        IMappingObject GetMapping<T>(object environment, string mappingName);

        IMappingObject GetMapping(Type type);
        IMappingObject GetMapping(Type type, object environment);
        IMappingObject GetMapping(Type type, string mappingName);
        IMappingObject GetMapping(Type type, object environment, string mappingName);

        IReadOnlyList<IMappingObject> GetAllMappings<T>();
        IReadOnlyList<IMappingObject> GetAllMappings(Type type);
        IReadOnlyList<IMappingObject> GetAllMappings();

        IReadOnlyList<IMappingObject> GetAllNamedMappings<T>();
        IReadOnlyList<IMappingObject> GetAllNamedMappings(Type type);
        IReadOnlyList<IMappingObject> GetAllNamedMappings();

        T GetMappingValue<T>();
        T GetMappingValue<T>(object environment);
        T GetMappingValue<T>(string mappingName);
        T GetMappingValue<T>(object environment, string mappingName);

        object GetMappingValue(Type type);
        object GetMappingValue(Type type, object environment);
        object GetMappingValue(Type type, string mappingName);
        object GetMappingValue(Type type, object environment, string mappingName);
    }
}
