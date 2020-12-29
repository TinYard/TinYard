using System;
using TinYard.API.Interfaces;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Extensions.EventSystem.Impl;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapExtension : IExtension
    {
        public object Environment { get { return _environment; } }
        private object _environment;

        private IContext _context;

        public MediatorMapExtension(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            _context = context;

            _context.PostConfigsInstalled += OnContextInitialized;
        }

        private void OnContextInitialized()
        {
            IViewRegister viewRegister = _context.Mapper.GetMappingValue<IViewRegister>();
            IInjector injector = _context.Mapper.GetMappingValue<IInjector>();

            MediatorMapper mediatorMapper = new MediatorMapper(_context);
            
            if (viewRegister != null && injector != null)
            {
                mediatorMapper = new MediatorMapper(_context, viewRegister);
            }

            _context.Mapper.Map<IMediatorMapper>().ToValue(mediatorMapper);
        }
    }
}