namespace TinYard.API.Interfaces
{
    public interface IMapper
    {
        IMapper Map<T>();
        IMapper ToValue(object value);

        T GetValue<T>();
    }
}
