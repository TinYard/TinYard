using System;
using TinYard.Impl.VO;

namespace TinYard.Framework.API.Interfaces
{
    public interface IMappingFactory : IFactory
    {
        T Build<T>() where T : class;
        object Build(Type valueType);
    }
}
