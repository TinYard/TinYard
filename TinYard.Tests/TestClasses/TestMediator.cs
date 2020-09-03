using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Extensions.ViewController.Tests.MockClasses;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestMediator : Mediator
    {
        [Inject]
        public TestView View;

        public TestMediator()
        {
        }

        public override void Configure()
        {
        }
    }
}
