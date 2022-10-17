using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;

namespace TinYard.Extensions.ViewController
{
    public class ViewControllerExtension : IExtension
    {
        public object Environment { get { return _environment; } }
        private object _environment;

        public ViewControllerExtension(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            ViewRegister viewRegister = new ViewRegister(context);

            context.Mapper.Map<IViewRegister>().ToSingleton(viewRegister);
        }
    }
}