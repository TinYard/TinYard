using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Impl.VO;

namespace TinYard.Framework.Impl.Factories
{
    public class MappingValueFactory : IMappingFactory
    {
        private IMapper _mapper;

        public MappingValueFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public object Build(params object[] args)
        {
            List<object> builtObjects = new List<object>();
            foreach(object argument in args)
            {
                if(argument is IMappingObject)
                {
                    builtObjects.Add(Build(argument as IMappingObject));
                }
                else if(argument is Type)
                {
                    builtObjects.Add(Build(argument as Type));
                }
            }

            return builtObjects.ToArray();
        }

        public T Build<T>() where T : class
        {
            return Build(typeof(T)) as T;
        }

        public object Build(Type valueType)
        {
            if (valueType == null)
                return null;

            return _mapper.GetMappingValue<IInjector>().CreateInjected(valueType);
        }
    }
}
