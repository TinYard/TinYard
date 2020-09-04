using System;
using System.Collections.Generic;

namespace TinYard.Extensions.ViewController.API.Interfaces
{
    public interface IViewRegister
    {
        event Action<IView> OnViewRegistered;
        IReadOnlyList<IView> RegisteredViews { get; }
    }
}
