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

        public string Name { get { return _name; } }
        private string _name = null;

        public event Action<IMappingObject> OnValueMapped;

        public Action<IMappingObject, Type> BuildDelegate { get; set; }
        private readonly Action<IMappingObject, Type> _defaultBuildDelegate;

        private IMapper _parentMapper;

        public MappingObject()
        {
            _defaultBuildDelegate = (obj, _) => obj.ToValue(_parentMapper?.MappingFactory?.Build(obj).MappedValue);
        }

        public MappingObject(IMapper parentMapper) : this()
        {
            _parentMapper = parentMapper;
        }

        public IMappingObject Map<T>()
        {
            return Map(typeof(T));
        }

        public IMappingObject Map<T>(string mappingName)
        {
            return Map(typeof(T), mappingName);
        }

        public IMappingObject Map(Type type)
        {
            _mappedType = type;

            return this;
        }

        public IMappingObject Map(Type type, string mappingName)
        {
            _mappedType = type;
            _name = mappingName;

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
            Type valueType = typeof(T);

            if (BuildDelegate != null)
            {
                BuildDelegate.Invoke(this, valueType);
            }
            else
            {
                _mappedValue = typeof(T);
                _defaultBuildDelegate.Invoke(this, valueType);
            }

            if (OnValueMapped != null)
                OnValueMapped.Invoke(this);

            return this;
        }
    }
}
