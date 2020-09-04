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

        public void Inject(object classToInjectInto)
        {
            //GetType as it's correct at run-time rather than compile time!
            Type classType = classToInjectInto.GetType();

            List<FieldInfo> injectables = InjectAttribute.GetInjectables(classType);

            foreach(FieldInfo field in injectables)
            {
                Type fieldType = field.FieldType;
                if(_mapper.GetMapping(fieldType) != null)
                {
                    field.SetValue(classToInjectInto, _mapper.GetMappingValue(fieldType));
                }
                else if(_extraInjectables.ContainsKey(fieldType))
                {
                    field.SetValue(classToInjectInto, _extraInjectables[fieldType]);
                }
            }
        }

        public void Inject(object target, object value)
        {
            List<FieldInfo> injectables = InjectAttribute.GetInjectables(target.GetType());

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
    }
}
