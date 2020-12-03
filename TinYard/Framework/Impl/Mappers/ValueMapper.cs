using System;
using System.Collections.Generic;
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

        protected List<IMappingObject> _mappingObjects = new List<IMappingObject>();

        public IMappingFactory MappingFactory { get { return _mappingFactory; } }
        protected IMappingFactory _mappingFactory;

        public ValueMapper()
        {
            _mappingFactory = new MappingValueFactory(this);
        }

        public IMappingObject Map<T>(string mappingName = null)
        {
            var mappingObj = new MappingObject(this).Map<T>(mappingName);

            if (OnValueMapped != null)
                mappingObj.OnValueMapped += ( mapping ) => OnValueMapped.Invoke(mapping);

            _mappingObjects.Add(mappingObj);
            return mappingObj;
        }

        public IMappingObject GetMapping<T>(string mappingName = null)
        {
            Type type = typeof(T);

            return GetMapping(type, mappingName);
        }

        public IMappingObject GetMapping(Type type, string mappingName = null)
        {
            List<IMappingObject> mappingsOfType = _mappingObjects.Where(mapping => mapping.MappedType.IsAssignableFrom(type)).ToList();

            IMappingObject value = null;

            //If we're looking for a specific name, try find it
            if(!string.IsNullOrWhiteSpace(mappingName))
            {
                value = mappingsOfType.FirstOrDefault(mapping =>
                {
                    if (string.IsNullOrWhiteSpace(mapping.Name))
                        return false;

                    return mapping.Name.Equals(mappingName);
                });
            }
            else if(mappingsOfType.Count() > 0)
            {
                //Else, grab the first one
                value = mappingsOfType[0];
            }

            return value;
        }

        public IReadOnlyList<IMappingObject> GetAllMappings()
        {
            return _mappingObjects.AsReadOnly();
        }

        public T GetMappingValue<T>(string mappingName = null)
        {
            var mappedValue = GetMapping<T>(mappingName)?.MappedValue;
            return mappedValue is T ? (T)mappedValue : default(T);
        }

        public object GetMappingValue(Type type, string mappingName = null)
        {
            return GetMapping(type, mappingName)?.MappedValue;
        }
    }
}
