﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Attributes;
using TinYard.Framework.Impl.VO;

namespace TinYard.Framework.Impl.Injectors
{
    public class TinYardInjector : IInjector
    {
        public object Environment { get { return _environment; } set { _environment = value; } }
        private object _environment;

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
            List<InjectableInformation> injectables = InjectAttribute.GetInjectableInformation(targetType);

            Type valueType = value.GetType();

            foreach(InjectableInformation injectable in injectables)
            {
                FieldInfo field = injectable.Field;
                Type fieldType = field.FieldType;
                if(valueType == fieldType || fieldType.IsAssignableFrom(valueType))
                {
                    field.SetValue(target, value);
                }
            }
        }

        private void InjectValues(object target, Type targetType)
        {
            List<InjectableInformation> injectables = InjectAttribute.GetInjectableInformation(targetType);

            foreach (InjectableInformation injectable in injectables)
            {
                FieldInfo field = injectable.Field;
                Type fieldType = field.FieldType;
                object valueToInject = GetInjectableValue(fieldType, injectable.Attribute.Name);

                if(valueToInject != null)
                {
                    Inject(valueToInject);
                    field.SetValue(target, valueToInject);
                }
            }
        }

        private object GetInjectableValue(Type valueType, string injectableName = null)
        {
            object injectableValue = null;

            var mapping = _mapper.GetMapping(valueType, Environment, injectableName);
            if (mapping != null)
            {
                injectableValue = mapping.MappedValue;
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
                //Get constructors that haven't got the attribute via filtering
                IEnumerable<ConstructorInfo> remainingConstructors = targetType.GetConstructors().Except(constructorsToCompare);
                targetConstructor = GetMostInjectableConstructor(remainingConstructors);
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
