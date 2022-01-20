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

        public bool IsMapped { get; private set; }

        public string Name { get { return _name; } }
        private string _name = null;

        public object Environment { get { return _environment; } }
        private object _environment;

        public event Action<IMappingObject> OnValueMapped;

        public Func<Type, object> BuildDelegate 
        { 
            get 
            {
                return _buildDelegate ?? _defaultBuildDelegate;
            }
            set { _buildDelegate = value; }
        }
        private Func<Type, object> _buildDelegate = null;
        private readonly Func<Type, object> _defaultBuildDelegate;

        private IMapper _parentMapper;

        public MappingObject()
        {
            _defaultBuildDelegate = (type) =>
                _parentMapper?.MappingFactory?.Build(type);
        }

        public MappingObject(object environment) : this()
        {
            _environment = environment;
        }

        public MappingObject(IMapper parentMapper) : this()
        {
            _parentMapper = parentMapper;
        }

        public MappingObject(IMapper parentMapper, object environment) : this()
        {
            _parentMapper = parentMapper;
            _environment = environment;
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

        public IMappingObject ToSingleton<T>(T value)
        {
            _mappedValue = value;

            OnValueMapped?.Invoke(this);

            return this;
        }
    
        public IMappingObject ToSingleton<T>()
        {
            Type valueType = typeof(T);

            _mappedValue = BuildDelegate.Invoke(valueType);

            OnValueMapped?.Invoke(this);

            return this;
        }
    }
}
