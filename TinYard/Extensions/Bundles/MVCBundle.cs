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
        public object Environment { get { return _environment; } }
        private object _environment;

        public MVCBundle(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            context
                .Install(new EventSystemExtension(Environment))
                .Install(new ViewControllerExtension(Environment))
                .Install(new MediatorMapExtension(Environment))
                .Install(new CommandSystemExtension(Environment));
        }
    }
}
