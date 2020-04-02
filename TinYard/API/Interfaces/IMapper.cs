using TinYard.Impl.VO;

namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        IMappingObject Map<T>();

        IMappingObject GetMapping<T>();
        object GetMappingValue<T>();
    }
}
