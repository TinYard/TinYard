using TinYard.API.Interfaces;
using TinYard.Extensions.CommandSystem;
using TinYard.Extensions.EventSystem;
using TinYard.Extensions.MediatorMap;
using TinYard.Extensions.ViewController;

namespace TinYard.Extensions.Bundles
{
    /// <summary>
    /// MVC Bundle includes:
    ///     Event System Extension
    ///     View Controller Extension
    ///     Mediator Map Extension
    ///     Command System Extension
    /// </summary>
    public class MVCBundle : IBundle
    {
        public void Install(IContext context)
        {
            context
                .Install(new EventSystemExtension())
                .Install(new ViewControllerExtension())
                .Install(new MediatorMapExtension())
                .Install(new CommandSystemExtension());
        }
    }
}
