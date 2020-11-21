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

        public TinYardInjector(IContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            _extraInjectables = new Dictionary<Type, object>();
        }

        public void AddInjectable(Type injectableType, object injectableObject)
        {
            _extraInjectables[injectableType] = injectableObject;
        }

        public T CreateInjected<T>()
        {
            Type targetType = typeof(T);
            return (T)CreateInjected(targetType);
        }

        public object CreateInjected(Type targetType)
        {
            //Prefer constructors with an attribute over non-attributed constructors
            ConstructorInfo bestMatchedConstructor = GetTargetConstructor(targetType);

            if (bestMatchedConstructor != null)
            {
                object[] parameters = CreateConstructorParameters(bestMatchedConstructor);

                object constructedObj = bestMatchedConstructor.Invoke(parameters);
                Inject(constructedObj);

                return constructedObj;
            }
            else
            {
                return null;
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
            List<FieldInfo> injectables = InjectAttribute.GetInjectableFields(targetType);

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
            List<FieldInfo> injectables = InjectAttribute.GetInjectableFields(targetType);

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

        private ConstructorInfo GetTargetConstructor(Type targetType)
        {
            List<ConstructorInfo> constructorsToCompare = InjectAttribute.GetInjectableConstructors(targetType);
            ConstructorInfo targetConstructor = GetMostInjectableConstructor(constructorsToCompare);

            //If no attributed constructors could be completed, lets try normal ones
            if (targetConstructor == null)
            {
                constructorsToCompare.Clear();
                constructorsToCompare.AddRange(targetType.GetConstructors());

                targetConstructor = GetMostInjectableConstructor(constructorsToCompare);
            }

            return targetConstructor;
        }

        private ConstructorInfo GetMostInjectableConstructor(IEnumerable<ConstructorInfo> constructorsToCompare)
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
    }
}
