using System;
using System.Collections.Generic;
using TinYard.API.Interfaces;

namespace TinYard.Impl.Mappers
{
    public class ValueMapper : IMapper
    {
        private Dictionary<Type, object> _mappings = new Dictionary<Type, object>();

        public T GetValue<T>()
        {
            object value;
            _mappings.TryGetValue(typeof(T), out value);

            return (T)value;
        }

        public IMapping Map<T>()
        {
            throw new System.NotImplementedException();
        }

        public IMapping ToValue(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
