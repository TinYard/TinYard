using TinYard.API.Interfaces;
using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;

namespace TinYard.Extensions.ViewController
{
    public class ViewControllerExtension : IExtension
    {
        public void Install(IContext context)
        {
            ViewRegister viewRegister = new ViewRegister(context);

            context.Mapper.Map<IViewRegister>().ToValue(viewRegister);
        }
    }
}