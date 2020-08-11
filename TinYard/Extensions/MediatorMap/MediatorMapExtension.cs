using TinYard.API.Interfaces;
using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Extensions.MediatorMap.Impl.Mappers;

namespace TinYard.Extensions.MediatorMap
{
    public class MediatorMapExtension : IExtension
    {
        public void Install(IContext context)
        {
            context.Mapper.Map<IMediatorMapper>().ToValue(new MediatorMapper());
        }
    }
}