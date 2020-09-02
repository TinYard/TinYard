using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Factories;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : IMediatorMapper
    {
        public event Action<IMediatorMappingObject> OnMediatorMapping;

        public IMediatorFactory MappingFactory { get { return _mappingFactory; } }
        protected IMediatorFactory _mappingFactory;

        private List<IMediatorMappingObject> _mappingObjects = new List<IMediatorMappingObject>();

        public MediatorMapper()
        {
            _mappingFactory = new MediatorFactory();
        }

        public IMediatorMappingObject Map<T>() where T : IView
        {
            var mappingObj = new MediatorMappingObject().Map<T>();

            if (OnMediatorMapping != null)
                mappingObj.OnMediatorMapped += (mapping) => OnMediatorMapping.Invoke(mapping);

            _mappingObjects.Add(mappingObj);

            return mappingObj;
        }

        public IMediatorMappingObject Map(IView view)
        {
            var mappingObj = new MediatorMappingObject().Map(view);

            if (OnMediatorMapping != null)
                mappingObj.OnMediatorMapped += (mapping) => OnMediatorMapping.Invoke(mapping);

            _mappingObjects.Add(mappingObj);

            return mappingObj;
        }

        public IMediatorMappingObject GetMapping<T>() where T : IView
        {
            return GetMapping(typeof(T));
        }

        public IMediatorMappingObject GetMapping(IView view)
        {
            return _mappingObjects.FirstOrDefault(mapping => mapping.View == view || mapping.ViewType == view.GetType());
        }

        private IMediatorMappingObject GetMapping(Type viewType)
        {
            return _mappingObjects.FirstOrDefault(mapping => mapping.View?.GetType() == viewType || mapping.ViewType == viewType);
        }

        public IReadOnlyList<IMediatorMappingObject> GetAllMappings()
        {
            return _mappingObjects.AsReadOnly();
        }

        public object GetMappingMediator<T>() where T : IView
        {
            return GetMapping<T>()?.Mediator;
        }

        public object GetMappingMediator(IView view)
        {
            return GetMapping(view)?.Mediator;
        }
    }
}
