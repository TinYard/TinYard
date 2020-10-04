using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class InterfaceTestMediator : Mediator
    {
        [Inject]
        public ITestView view;

        public override void Configure()
        {
            AddViewListener(TestEvent.Type.Test1, () => Dispatch(new TestEvent(TestEvent.Type.Test2)));
        }
    }
}
