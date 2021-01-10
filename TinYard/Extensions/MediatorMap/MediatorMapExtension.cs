using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapExtension : IExtension
    {
        private IContext _context;

        public void Install(IContext context)
        {
            _context = context;

            var viewRegister = _context.Mapper.GetMappingValue<IViewRegister>();

            MediatorMapper mediatorMapper = new MediatorMapper(context, viewRegister);
            _context.Mapper.Map<IMediatorMapper>().ToValue(mediatorMapper);
        }
    }
}