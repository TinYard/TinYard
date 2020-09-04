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
            }

            return builtObjects.ToArray();
        }

        public IMappingObject Build(IMappingObject mappingObject)
        {
            Type type = mappingObject.MappedValue as Type;

            //In the odd case that the factory is being used when an actual value is already set, we'll get its type
            if (type == null)
                type = mappingObject.MappedValue.GetType();

            //Last resort, hope it's not an interface
            if (type == null)
                type = mappingObject.MappedType;

            object[] constructorDependencies = GetConstructorDependencies(type);

            //If we have any null objects, we can break something further down the line.
            //Making the array null means we should use the default constructor instead when possible
            if (constructorDependencies.Any().GetType() == null)
                constructorDependencies = null;

            object value = Activator.CreateInstance(type, constructorDependencies);
            return mappingObject.ToValue(value);
        }

        private object[] GetConstructorDependencies(Type type)
        {
            ParameterInfo[] constructorInfo = type.GetConstructors().FirstOrDefault().GetParameters();

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
