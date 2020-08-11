using TinYard.Extensions.MediatorMap.API.Interfaces;
using TinYard.Impl.Mappers;

namespace TinYard.Extensions.MediatorMap.Impl.Mappers
{
    public class MediatorMapper : ValueMapper, IMediatorMapper
    {
        public MediatorMapper()
        {
            //_mappingFactory = new MediatorFactory(this);
        }
    }
}
