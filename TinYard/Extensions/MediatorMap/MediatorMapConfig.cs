using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapConfig : IConfig
    {
        [Inject]
        public IContext context;

        [Inject]
        public IViewRegister viewRegister;

        public void Configure()
        {
            MediatorMapper mediatorMapper = new MediatorMapper(context, viewRegister);
            context.Mapper.Map<IMediatorMapper>().ToValue(mediatorMapper);
        }
    }
}
