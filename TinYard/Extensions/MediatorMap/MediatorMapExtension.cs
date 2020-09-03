using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;

            _context.PostConfigsInstalled += OnContextInitialized;
        }

        private void OnContextInitialized()
        {
            IViewRegister viewRegister = _context.Mapper.GetMapping<IViewRegister>()?.MappedValue as IViewRegister;
            IInjector injector = _context.Mapper.GetMappingValue<IInjector>() as IInjector;

            MediatorMapper mediatorMapper = new MediatorMapper();
            
            if (viewRegister != null && injector != null)
            {
                mediatorMapper = new MediatorMapper(injector, viewRegister);
            }

            _context.Mapper.Map<IMediatorMapper>().ToValue(mediatorMapper);
        }
    }
}