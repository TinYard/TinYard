namespace TinYard.API.Interfaces
{
    public interface IMapping
    {
        T GetValue<T>();

        IMapping Map<T>();
        IMapping ToValue(object value);
    }
}
