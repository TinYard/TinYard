using System;

namespace TinYard.Impl.VO
{
    public interface IMappingObject
    {
        Type MappedType { get; }
        object MappedValue { get; }

        IMappingObject Map<T>();

        IMappingObject ToValue<T>();
        IMappingObject ToValue(object value);
    }
}
