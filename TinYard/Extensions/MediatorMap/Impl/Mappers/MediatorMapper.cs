using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.Mappers;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : IMediatorMapper
    {
        public event Action<IMediatorMappingObject> OnValueMapped;

        public IMappingFactory MappingFactory => throw new NotImplementedException();

        private List<IMediatorMappingObject> _mappingObjects = new List<IMediatorMappingObject>();

        public MediatorMapper()
        {
            //_mappingFactory = new MediatorFactory(this);
        }

        public IMediatorMappingObject Map<T2>(bool autoInitializeValue = false)
        {
            var mappingObj = new MediatorMappingObject().Map<T2>();

            //TODO 
            // Add Factory use

            if (OnValueMapped != null)
                mappingObj.OnViewMapped += (mapping) => OnValueMapped.Invoke(mapping);

            _mappingObjects.Add(mappingObj);

            return mappingObj;
        }

        public IMediatorMappingObject GetMapping<T2>()
        {
            return GetMapping(typeof(T2));
        }

        public IMediatorMappingObject GetMapping(Type type)
        {
            return _mappingObjects.FirstOrDefault(mapping => mapping.View.IsAssignableFrom(type));
        }

        public IReadOnlyList<IMediatorMappingObject> GetAllMappings()
        {
            return _mappingObjects.AsReadOnly();
        }

        public object GetMappingValue<T2>()
        {
            return GetMappingValue(typeof(T2));
        }

        public object GetMappingValue(Type type)
        {
            return GetMapping(type)?.Mediator;
        }
    }
}
