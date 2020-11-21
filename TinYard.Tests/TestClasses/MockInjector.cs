using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Tests.TestClasses
{
    public class MockInjector : IInjector
    {
        public void AddInjectable(Type injectableType, object injectableObject)
        {
        }

        public T CreateInjected<T>()
        {
            return (T)CreateInjected(typeof(T));
        }

        public object CreateInjected(Type targetType)
        {
            return Activator.CreateInstance(targetType);
        }

        public void Inject(object target)
        {
            
        }

        public void Inject(object target, object value)
        {
            
        }
    }
}
