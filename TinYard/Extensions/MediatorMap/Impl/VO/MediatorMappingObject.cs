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

        public IMediatorMappingObject Map<T>()
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

        public IMediatorMappingObject Map(object view)
        {
            Type viewType = view.GetType();

            if(viewType.IsInterface)
            {
                if (!typeof(IView).IsAssignableFrom(viewType))
                    return this;
            }
            else if (!typeof(IView).IsAssignableFrom(viewType))
                return this;

            View = view as IView;
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
