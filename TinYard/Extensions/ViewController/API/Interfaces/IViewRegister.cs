using System.Collections.Generic;

namespace TinYard.Extensions.ViewController.API.Interfaces
{
    public interface IViewRegister
    {
        IReadOnlyList<IView> RegisteredViews { get; }
    }
}
