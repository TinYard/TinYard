using System;

namespace TinYard.Impl.VO
{
    public interface IMappingObject
    {
        Type MappedType { get; }
        object MappedValue { get; }

        event Action<IMappingObject> OnValueMapped;

        IMappingObject Map<T>();
        IMappingObject Map(Type type);

        IMappingObject ToValue(object value);
        IMappingObject BuildValue<T>();
    }
}