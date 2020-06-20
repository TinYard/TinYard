using System;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Framework.Impl.Factories
{
    public class MappingValueFactory<T> : IMappingFactory<T>
    {
        private IMapper _mapper;
        private Type _type;

        public MappingValueFactory(IMapper mapper)
        {
            _mapper = mapper;
            _type = typeof(T);
        }

        public T Build()
        {
            T value = (T)Activator.CreateInstance(_type);
            return value;
        }
    }
}
