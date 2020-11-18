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
                if (_mapper.GetMapping(fieldType) != null)
                {
                    var valueToInject = _mapper.GetMappingValue(fieldType);
                    Inject(valueToInject);

                    field.SetValue(target, _mapper.GetMappingValue(fieldType));
                }
                else if (_extraInjectables.ContainsKey(fieldType))
                {
                    var valueToInject = _extraInjectables[fieldType];
                    Inject(valueToInject);
                    field.SetValue(target, valueToInject);
                }
            }
        }
    }
}
