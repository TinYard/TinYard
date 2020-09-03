using System;
using System.Collections.Generic;
using TinYard.Extensions.MediatorMap.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.Impl.Factories
{
    public class MediatorFactory : IMediatorFactory
    {
        public IMediator Build(Type mediatorType)
        {
            if (!typeof(IMediator).IsAssignableFrom(mediatorType) || mediatorType == null)
                return null;

            return Activator.CreateInstance(mediatorType) as IMediator;
        }

        public T Build<T>() where T : IMediator
        {
            return (T)Build(typeof(T));
        }

        public object Build(params object[] args)
        {
            List<IMediator> mediators = new List<IMediator>();

            foreach(object arg in args)
            {
                mediators.Add(Build(arg.GetType()));
            }

            return mediators;
        }
    }
}
