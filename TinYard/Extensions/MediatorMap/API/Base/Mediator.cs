using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.MediatorMap.API.Base
{
    public abstract class Mediator : IMediator
    {
        [Inject]
        public IEventDispatcher Dispatcher;

        /// <summary>
        /// Add your listeners here. This will be called once the Mediator has been injected into
        /// </summary>
        public abstract void Configure();
    }
}
