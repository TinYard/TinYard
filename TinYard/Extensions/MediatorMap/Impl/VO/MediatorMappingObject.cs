using System;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Impl.VO;

namespace TinYard.Extensions.MediatorMap.Impl.VO
{
    public class MediatorMappingObject : IMediatorMappingObject
    {
        public IView View { get; private set; }

        public Type ViewType
        {
            get
            {
                return _internalMappingObject.MappedType;
            }
        }

        public IMediator Mediator 
        { 
            get
            {
                return _internalMappingObject.MappedValue as IMediator;
            }
        }

        public event Action<IMediatorMappingObject> OnMediatorMapped;

        private IMappingObject _internalMappingObject;

        public MediatorMappingObject(IMappingObject internalMappingObject = null)
        {
            _internalMappingObject = internalMappingObject ?? new MappingObject();//If null, create a new mapping obj
            _internalMappingObject.OnValueMapped += (mappingObj) => OnMediatorMapped?.Invoke(this);
        }

        public IMediatorMappingObject Map<T>() where T : IView
        {
            _internalMappingObject.Map<T>();

            return this;
        }

        public IMediatorMappingObject Map(IView view)
        {
            _internalMappingObject.Map(view.GetType());

            View = view;

            return this;
        }

        public IMediatorMappingObject ToMediator<T>() where T : IMediator
        {
            _internalMappingObject.ToValue<T>();

            return this;
        }

        public IMediatorMappingObject ToMediator(IMediator mediator)
        {
            _internalMappingObject.ToValue(mediator);

            return this;
        }
    }
}
