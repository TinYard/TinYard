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

        IMappingObject Map<T>(string mappingName = null);
        IMappingObject Map(Type type, string mappingName = null);

        IMappingObject ToValue(object value);
        IMappingObject BuildValue<T>();
    }
}