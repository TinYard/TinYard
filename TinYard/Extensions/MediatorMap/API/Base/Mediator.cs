using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.MediatorMap.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Base
{
    public class Mediator : EventDispatcher, IMediator
    {
        /// <summary>
        /// Add your listeners here. This will be called once the Mediator has been injected into
        /// </summary>
        public virtual void Configure() { }
    }
}
