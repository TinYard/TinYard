using TinYard.Impl.VO;

namespace TinYard.Framework.API.Interfaces
{
    public interface IMappingFactory
    {
        IMappingObject Build(IMappingObject mappingObject);
    }
}
