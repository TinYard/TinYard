using System;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.VO
{
    public class MediatorMappingObject : IMediatorMappingObject
    {
        public Type View { get; private set; }

        public Type Mediator { get; private set; }

        public event Action<IMediatorMappingObject> OnViewMapped;

        public IMediatorMappingObject Map<T>()
        {
            View = typeof(T);

            return this;
        }

        public IMediatorMappingObject ToMediator<T>() where T : IMediator
        {
            Mediator = typeof(T);

            return this;
        }

        public IMediatorMappingObject ToMediator(IMediator mediator)
        {
            Mediator = mediator.GetType();

            return this;
        }
    }
}
