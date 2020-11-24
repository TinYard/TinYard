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

        public Action<IMappingObject> BuildDelegate { get; set; }
        private readonly Action<IMappingObject> _defaultBuildDelegate;

        private IMapper _parentMapper;

        public MappingObject()
        {
            _defaultBuildDelegate = (obj) => obj.ToValue(_parentMapper?.MappingFactory?.Build(obj).MappedValue);
        }

        public MappingObject(IMapper parentMapper) : this()
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

            if (BuildDelegate != null)
            {
                BuildDelegate.Invoke(this);
            }
            else
            {
                _defaultBuildDelegate.Invoke(this);
            }

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }
    }
}
