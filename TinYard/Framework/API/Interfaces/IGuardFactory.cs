using System;
using TinYard.Framework.API.Base;

namespace TinYard.Framework.API.Interfaces
{
    public interface IGuardFactory : IFactory
    {
        IGuard Build<T>() where T : Guard;
        IGuard Build(Type guardType);
    }
}
