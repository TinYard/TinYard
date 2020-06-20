using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Framework.Impl.Factories
{
    public class MappingValueFactory<T> : IMappingFactory<T>
    {
        public T Build()
        {
            throw new NotImplementedException();
        }
    }
}
