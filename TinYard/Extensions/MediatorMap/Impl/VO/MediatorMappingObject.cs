using System;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.VO;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.VO
{
    public class MediatorMappingObject : IMediatorMappingObject
    {
        public IView View { get; private set; }
        public IMediator Mediator { get; private set; }

        public Type ViewType { get; private set; }
        public Type MediatorType { get; private set; }


        public event Action<IMediatorMappingObject> OnMediatorMapped;

        public IMediatorMappingObject Map<T>() where T : IView
        {
            ViewType = typeof(T);

            return this;
        }

        public IMediatorMappingObject Map(IView view)
        {
            View = view;
            ViewType = view.GetType();

            return this;
        }

        public IMediatorMappingObject ToMediator<T>() where T : IMediator
        {
            MediatorType = typeof(T);

            OnMediatorMapped?.Invoke(this);

            return this;
        }

        public IMediatorMappingObject ToMediator(IMediator mediator)
        {
            Mediator = mediator;
            MediatorType = mediator.GetType();

            OnMediatorMapped?.Invoke(this);

            return this;
        }
    }
}
