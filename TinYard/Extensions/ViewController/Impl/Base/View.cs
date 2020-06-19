using TinYard.Extensions.ViewController.API.Interfaces;

namespace TinYard.Extensions.ViewController.Impl.Base
{
    public class View : IView
    {
        public View()
        {
            Register();
        }

        protected virtual void Register()
        {
            ViewRegister.Register(this);
        }
    }
}
