using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Factories;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : IMediatorMapper
    {
        public event Action<IMediatorMappingObject> OnMediatorMapping;

        public IMediatorFactory MediatorFactory { get { return _mediatorFactory; } }
        protected IMediatorFactory _mediatorFactory;

        private List<IMediatorMappingObject> _mappingObjects = new List<IMediatorMappingObject>();

        private IViewRegister _viewRegister;
        private IInjector _injector;

        public MediatorMapper(IInjector injector, IViewRegister viewRegister) : this()
        {
            _injector = injector;

            _viewRegister = viewRegister;
            _viewRegister.OnViewRegistered += OnViewRegistered;
        }

        public MediatorMapper()
        {
            _mediatorFactory = new MediatorFactory();
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

        private void OnViewRegistered(IView view)
        {
            IMediatorMappingObject mapping = GetMapping(view);

            IMediator builtMediator = _mediatorFactory.Build(mapping.Mediator.GetType());

            _injector.Inject(builtMediator);

            builtMediator.Configure();
        }
    }
}
