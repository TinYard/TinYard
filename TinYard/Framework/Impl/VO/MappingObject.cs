using System;

namespace TinYard.Impl.VO
{
    public class MappingObject : IMappingObject
    {
        public Type MappedType { get { return _mappedType; } }
        private Type _mappedType;
     
        public object MappedValue { get { return _mappedValue; } }
        private object _mappedValue;

        public event Action<IMappingObject> OnValueMapped;

        public IMappingObject Map<T>()
        {
            _mappedType = typeof(T);

            return this;
        }

        public IMappingObject ToValue<T>()
        {
            _mappedValue = typeof(T);

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }

        public IMappingObject ToValue(object value)
        {
            _mappedValue = value;

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }
    }
}
