using System;

namespace TinYard.Impl.VO
{
    public interface IMappingObject
    {
        Type MappedType { get; }
        object MappedValue { get; }

        string Name { get; }
        object Environment { get; }

        event Action<IMappingObject> OnValueMapped;

        Func<Type, object> BuildDelegate { get; set; }

        IMappingObject Map<T>();
        IMappingObject Map<T>(string mappingName);

        IMappingObject Map(Type type);
        IMappingObject Map(Type type, string mappingName);

        IMappingObject ToSingleton<T>(T value);

        IMappingObject ToSingleton<T>();
    }
}