using System;

namespace TinYard.Impl.VO
{
    public interface IMappingObject
    {
        Type MappedType { get; }
        object MappedValue { get; }

        string Name { get; }

        event Action<IMappingObject> OnValueMapped;

        Action<IMappingObject, Type> BuildDelegate { get; set; }

        IMappingObject Map<T>();
        IMappingObject Map<T>(string mappingName);

        IMappingObject Map(Type type);
        IMappingObject Map(Type type, string mappingName);

        IMappingObject ToValue(object value);
        IMappingObject BuildValue<T>();
    }
}