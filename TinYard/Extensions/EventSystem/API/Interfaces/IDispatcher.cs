namespace TinYard.Extensions.EventSystem.API.Interfaces
{
    public interface IDispatcher
    {
        void Dispatch(IEvent evt);
    }
}
