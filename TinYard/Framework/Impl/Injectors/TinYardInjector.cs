using System;
using System.Collections.Generic;
using System.Reflection;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Framework.Impl.Injectors
{
    public class TinYardInjector : IInjector
    {
        private IContext _context;
        private IMapper _mapper;

        private Dictionary<Type, object> _extraInjectables;

        public TinYardInjector(IContext context)
        {
            _context = context;
            _mapper = _context.Mapper;

            _extraInjectables = new Dictionary<Type, object>();
        }

        public void AddInjectable(Type injectableType, object injectableObject)
        {
            _extraInjectables[injectableType] = injectableObject;
        }

        public T Inject<T>()
        {
            Type targetType = typeof(T);

            ConstructorInfo[] constructors = targetType.GetConstructors();

            ConstructorInfo bestMatchedConstructor = GetBestInjectableConstructor(constructors);

            if(bestMatchedConstructor != null)
            {
                object[] parameters = CreateConstructorParameters(bestMatchedConstructor);

                return (T)bestMatchedConstructor.Invoke(parameters);
            }
            else
            {
                return default(T);
            }
        }

        public void Inject(object target)
        {
            //GetType as it's correct at run-time rather than compile time!
            Type targetType = target.GetType();

            InjectValues(target, targetType);
        }

        public void Inject(object target, object value)
        {
            Type targetType = target.GetType();
            List<FieldInfo> injectables = InjectAttribute.GetInjectables(targetType);

            Type valueType = value.GetType();

            foreach(FieldInfo field in injectables)
            {
                Type fieldType = field.FieldType;

                if(valueType == fieldType || fieldType.IsAssignableFrom(valueType))
                {
                    field.SetValue(target, value);
                }
            }
        }

        private void InjectValues(object target, Type targetType)
        {
            List<FieldInfo> injectables = InjectAttribute.GetInjectables(targetType);

            foreach (FieldInfo field in injectables)
            {
                Type fieldType = field.FieldType;
                object valueToInject = GetInjectableValue(fieldType);

                if(valueToInject != null)
                {
                    Inject(valueToInject);
                    field.SetValue(target, valueToInject);
                }
            }
        }

        private object GetInjectableValue(Type valueType)
        {
            object injectableValue = null;

            if (_mapper.GetMapping(valueType) != null)
            {
                injectableValue = _mapper.GetMappingValue(valueType);
            }
            else if (_extraInjectables.ContainsKey(valueType))
            {
                injectableValue = _extraInjectables[valueType];
            }

            return injectableValue;
        }

        private ConstructorInfo GetBestInjectableConstructor(ConstructorInfo[] constructorsToCompare)
        {
            ConstructorInfo bestInjectableConstructor = null;

            foreach (ConstructorInfo constructor in constructorsToCompare)
            {
                ParameterInfo[] constructorParams = constructor.GetParameters();

                bool canInjectAllParams = true;

                //Check if we can inject all the parameters
                foreach (ParameterInfo parameterInfo in constructorParams)
                {
                    if (parameterInfo.IsOut)
                    {
                        canInjectAllParams = false;
                        break;
                    }

                    Type paramType = parameterInfo.ParameterType;

                    //We can't inject a value for this parameter if there's nothing in our magic hat to pull out into it
                    if (_mapper.GetMapping(paramType) == null && !(_extraInjectables.ContainsKey(paramType)))
                    {
                        canInjectAllParams = false;
                        break;
                    }
                }

                //We match more parameters, this is our new choice
                if (canInjectAllParams && (bestInjectableConstructor == null || constructorParams.Length > bestInjectableConstructor.GetParameters().Length))
                {
                    bestInjectableConstructor = constructor;
                }
            }

            return bestInjectableConstructor;
        }

        private object[] CreateConstructorParameters(ConstructorInfo constructorInfo)
        {
            ParameterInfo[] constructorParams = constructorInfo.GetParameters();
            object[] parameters = new object[constructorParams.Length];

            for (int i = 0; i < constructorParams.Length; i++)
            {
                object value = GetInjectableValue(constructorParams[i].ParameterType);
                Inject(value);

                parameters[i] = value;
            }

            return parameters;
        }
    }
}
