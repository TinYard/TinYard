using System;
using System.Reflection;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.MediatorMap.API.Base
{
    public abstract class Mediator : IMediator
    {
        [Inject]
        public IEventDispatcher Dispatcher;

        public IView ViewComponent
        {
            get
            {
                return _view;
            }
            set 
            { 
                _view = value;
                _viewDispatcher = GetDispatcher(_view);
            }
        }

        private IView _view;
        private IEventDispatcher _viewDispatcher;

        /// <summary>
        /// Add your listeners here. This will be called once the Mediator has been injected into
        /// </summary>
        public abstract void Configure();

        private IEventDispatcher GetDispatcher(object dispatcherContainer)
        {
            if (dispatcherContainer == null)
                return null;

            if (dispatcherContainer is IEventDispatcher)
                return dispatcherContainer as IEventDispatcher;

            Type dispatcherContainerType = dispatcherContainer.GetType();

            PropertyInfo[] properties = dispatcherContainerType.GetProperties
                (
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.FlattenHierarchy
                );

            foreach(PropertyInfo property in properties)
            {
                var propertyValue = property.GetValue(dispatcherContainer);
                if(propertyValue is IEventDispatcher)
                {
                    return propertyValue as IEventDispatcher;
                }
            }

            //No properties of that type..
            //Try Fields
            FieldInfo[] fields = dispatcherContainerType.GetFields
                (
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.FlattenHierarchy
                );

            foreach (FieldInfo field in fields)
            {
                var fieldValue = field.GetValue(dispatcherContainer);
                if(fieldValue is IEventDispatcher)
                {
                    return fieldValue as IEventDispatcher;
                }
            }

            //Seems there is no IEventDispatcher!
            return null;
        }
    }
}
