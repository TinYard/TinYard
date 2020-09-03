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
            var viewRegister = _context.Mapper.GetMappingValue<IViewRegister>() as IViewRegister;
            var injector = _context.Mapper.GetMappingValue<IInjector>() as IInjector;

            if (viewRegister == null || injector == null)
                return;

            var mediatorMapper = new MediatorMapper(injector, viewRegister);
            _context.Mapper.Map<IMediatorMapper>().ToValue(mediatorMapper);
        }
    }
}