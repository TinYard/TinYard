using System;

namespace TinYard.Impl.VO
{
    public interface IMappingObject
    {
        Type MappedType { get; }
        object MappedValue { get; }

        event Action<IMappingObject> OnValueMapped;

        IMappingObject Map<T>();

        IMappingObject ToValue<T>();
        IMappingObject ToValue(object value);
    }
}
