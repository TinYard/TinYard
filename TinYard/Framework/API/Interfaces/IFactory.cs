namespace TinYard.Framework.API.Interfaces
{
    public interface IFactory
    {
        object Build(params object[] args);
    }
}
