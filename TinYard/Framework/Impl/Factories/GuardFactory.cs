using System;
using System.Collections.Generic;
using TinYard.Framework.API.Base;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Framework.Impl.Factories
{
    public class GuardFactory : IGuardFactory
    {
        public object Build(params object[] args)
        {
            List<IGuard> builtGuards = new List<IGuard>();

            foreach(object argument in args)
            {
                if (argument is Guard)
                    builtGuards.Add(Build(argument.GetType());
            }

            return builtGuards;
        }

        public IGuard Build<T>() where T : Guard
        {
            return Build(typeof(T));
        }

        public IGuard Build(Type guardType)
        {
            //Must be type of Guard
            if (!guardType.IsAssignableFrom(typeof(Guard)))
                return null;

            return Activator.CreateInstance(guardType) as IGuard;
        }
    }
}
