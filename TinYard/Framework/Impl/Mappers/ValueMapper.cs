using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Factories;
using TinYard.Impl.VO;

namespace TinYard.Impl.Mappers
{
    public class ValueMapper : IMapper
    {
        public event Action<IMappingObject> OnValueMapped;

        private List<IMappingObject> _mappingObjects = new List<IMappingObject>();

        private IMappingFactory _mappingFactory;

        public ValueMapper()
        {
            _mappingFactory = new MappingValueFactory(this);
        }

        public IMappingObject Map<T>(bool autoCreateValue = false)
        {
            var mappingObj = new MappingObject().Map<T>();

            if(autoCreateValue)
            {
                mappingObj = mappingObj.ToValue<T>();
                mappingObj = _mappingFactory.Build(mappingObj);
            }


            if (OnValueMapped != null)
                mappingObj.OnValueMapped += ( mapping ) => OnValueMapped.Invoke(mapping);

            _mappingObjects.Add(mappingObj);
            return mappingObj;
        }

        public IMappingObject GetMapping<T>()
        {
            Type type = typeof(T);

            return GetMapping(type);
        }

        public IMappingObject GetMapping(Type type)
        {
            var value = _mappingObjects.First(mapping => mapping.MappedType.IsAssignableFrom(type));

            return value;
        }

        public IReadOnlyList<IMappingObject> GetAllMappings()
        {
            return _mappingObjects.AsReadOnly();
        }

        public object GetMappingValue<T>()
        {
            return GetMapping<T>().MappedValue;
        }

        public object GetMappingValue(Type type)
        {
            return GetMapping(type).MappedValue;
        }
    }
}
