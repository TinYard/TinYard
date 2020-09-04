using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.ViewController.Impl.Base
{
    public class View : IView
    {
        public IEventDispatcher Dispatcher
        {
            get
            {
                return _dispatcher;
            }
            set
            {
                _dispatcher = value;
            }
        }

        private IEventDispatcher _dispatcher;

        public View()
        {
            _dispatcher = new EventDispatcher();
            Register();
        }

        protected virtual void Register()
        {
            ViewRegister.Register(this);
        }

        protected virtual void Dispatch(IEvent evt)
        {
            _dispatcher.Dispatch(evt);
        }
    }
}