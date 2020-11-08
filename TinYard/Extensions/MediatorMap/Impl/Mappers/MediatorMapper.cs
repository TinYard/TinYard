using System;
using System.Collections.Generic;
using System.Linq;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.VO;
using TinYard.Extensions.MediatorMap.Impl.Factories;
using TinYard.Extensions.MediatorMap.Impl.VO;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : IMediatorMapper
    {
        public event Action<IMediatorMappingObject> OnMediatorMapping;

        public IMediatorFactory MediatorFactory { get { return _mediatorFactory; } }
        protected IMediatorFactory _mediatorFactory;

        private List<IMediatorMappingObject> _mappingObjects = new List<IMediatorMappingObject>();

        private IContext _context;
        private IInjector _injector;
        private IViewRegister _viewRegister;

        public MediatorMapper(IContext context, IViewRegister viewRegister) : this(context)
        {
            _viewRegister = viewRegister;
            _viewRegister.OnViewRegistered += OnViewRegistered;
        }

        public MediatorMapper(IContext context)
        {
            _context = context;
            _injector = _context.Injector;

            _mediatorFactory = new MediatorFactory();
        }

        public IMediatorMappingObject Map<T>()
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

        public IMediatorMappingObject Map(object view)
        {
            var mappingObj = new MediatorMappingObject().Map(view);

            if (OnMediatorMapping != null)
                mappingObj.OnMediatorMapped += (mapping) => OnMediatorMapping.Invoke(mapping);

            _mappingObjects.Add(mappingObj);

            return mappingObj;
        }

        public IEnumerable<IMediatorMappingObject> GetMappings<T>()
        {
            return GetMappings(typeof(T));
        }

        public IEnumerable<IMediatorMappingObject> GetMappings(IView view)
        {
            Type viewType = view.GetType();

            return _mappingObjects.FindAll(
                mapping => 
                mapping.View == view || 
                mapping.ViewType == viewType ||
                mapping.ViewType.IsAssignableFrom(viewType)
            );
        }

        private IEnumerable<IMediatorMappingObject> GetMappings(Type viewType)
        {
            return _mappingObjects.FindAll(
                mapping => 
                mapping.View?.GetType() == viewType ||
                mapping.ViewType == viewType ||
                mapping.ViewType.IsAssignableFrom(viewType)
                );
        }

        public IReadOnlyList<IMediatorMappingObject> GetAllMappings()
        {
            return _mappingObjects.AsReadOnly();
        }

        private void OnViewRegistered(IView view)
        {
            IEnumerable<IMediatorMappingObject> mappings = GetMappings(view);
            if (mappings == null || mappings.Count() <= 0)
                return;
            
            foreach(IMediatorMappingObject mapping in mappings)
            {
                Type mediatorType;
                
                if (mapping.Mediator != null)
                    mediatorType = mapping.Mediator.GetType();
                else if (mapping.MediatorType != null)
                    mediatorType = mapping.MediatorType;
                else
                    return;
                
                IMediator mediator = _mediatorFactory.Build(mediatorType);
                
                mediator.ViewComponent = view;
                _injector.Inject(mediator, view);
                _injector.Inject(mediator);//Ensure any other injections are provided too
                
                mediator.Configure();
            }

        }
    }
}
