using System;
using System.Collections.Generic;
using System.Reflection;
using TinYard.API.Interfaces;
using TinYard.Framework.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Framework.Impl.Injectors
{
    public class Injector : IInjector
    {
        IContext _context;
        IMapper _mapper;

        public Injector(IContext context)
        {
            _context = context;
            _mapper = _context.Mapper;
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
            }
        }
    }
}
