using TinYard.Impl.VO;

namespace TinYard.Framework.API.Interfaces
{
    public interface IMappingFactory : IFactory
    {
        IMappingObject Build(IMappingObject mappingObject);
    }
}
