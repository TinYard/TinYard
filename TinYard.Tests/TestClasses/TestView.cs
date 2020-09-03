using TinYard.Extensions.ViewController.API.Interfaces;
using TinYard.Extensions.ViewController.Impl.Base;

namespace TinYard.Extensions.ViewController.Tests.MockClasses
{
    public class TestView : IView
    {
        public TestView()
        {
            //Register if possible
            ViewRegister.Register(this);
        }
    }
}
