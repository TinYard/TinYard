using System;
using System.Collections.Generic;
using TinYard.Framework.API.Interfaces;

namespace TinYard.API.Interfaces
{
    public interface IMapper<T1>
    {
        event Action<T1> OnValueMapped;
        IMappingFactory MappingFactory { get; }

        T1 Map<T2>(bool autoInitializeValue = false);

        T1 GetMapping<T2>();
        T1 GetMapping(Type type);
        IReadOnlyList<T1> GetAllMappings();

        object GetMappingValue<T2>();
        object GetMappingValue(Type type);
    }
}
