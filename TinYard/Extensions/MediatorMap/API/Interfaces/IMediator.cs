using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.MediatorMap.API.Interfaces
{
    public interface IMediator
    {
        IView ViewComponent { get; set; }

        void Configure();
    }
}
