using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            object[] constructorDependencies = GetConstructorDependencies();

            //If we have any null objects, we can break something further down the line.
            //Making the array null means we should use the default constructor instead when possible
            if (constructorDependencies.Any().GetType() == null)
                constructorDependencies = null;

            T value = (T)Activator.CreateInstance(_type, constructorDependencies);
            return value;
        }

        private object[] GetConstructorDependencies()
        {
            ParameterInfo[] constructorInfo = _type.GetConstructors().FirstOrDefault().GetParameters();

            List<object> constructorParams = new List<object>();
            foreach(ParameterInfo constructorParameter in constructorInfo)
            {
                object value = GetValueFromMapper(constructorParameter.ParameterType);
                constructorParams.Add(value);
            }

            return constructorParams.ToArray();
        }

        private object GetValueFromMapper(Type parameterType)
        {
            return _mapper.GetMappingValue(parameterType);
        }
    }
}
