namespace TinYard.API.Interfaces
{
    public interface IExtension
    {
        object Environment { get; }

        void Install(IContext context);
    }
}
