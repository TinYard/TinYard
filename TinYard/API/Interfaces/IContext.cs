namespace TinYard.API.Interfaces
{
    public interface IContext
    {
        IContext Install(IExtension extension);
        
        void Initialize();

        bool ContainsExtension(IExtension extension);
    }
}
