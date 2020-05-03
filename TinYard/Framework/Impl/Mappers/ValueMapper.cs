using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Impl.VO;

namespace TinYard.Impl.Mappers
{
    public class ValueMapper : IMapper
    {
        private List<IMappingObject> _mappingObjects = new List<IMappingObject>();

        public IMappingObject Map<T>()
        {
            var mappingObj = new MappingObject();
            _mappingObjects.Add(mappingObj);
            return mappingObj.Map<T>();
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
