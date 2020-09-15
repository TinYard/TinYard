using System;
using TinYard.API.Interfaces;

namespace TinYard.Impl.VO
{
    public class MappingObject : IMappingObject
    {
        public Type MappedType { get { return _mappedType; } }
        private Type _mappedType = null;
     
        public object MappedValue { get { return _mappedValue; } }
        private object _mappedValue = null;

        public event Action<IMappingObject> OnValueMapped;

        private IMapper _parentMapper;

        public MappingObject()
        {

        }

        public MappingObject(IMapper parentMapper)
        {
            _parentMapper = parentMapper;
        }

        public IMappingObject Map<T>()
        {
            return Map(typeof(T));
        }

        public IMappingObject Map(Type type)
        {
            _mappedType = type;

            return this;
        }

        public IMappingObject ToValue(object value)
        {
            _mappedValue = value;

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }

        public IMappingObject BuildValue<T>()
        {
            _mappedValue = typeof(T);
            
            _mappedValue = _parentMapper?.MappingFactory?.Build(this).MappedValue;

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }
    }
}
