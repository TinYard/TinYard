using System;
using TinYard.Extensions.EventSystem.Tests.MockClasses;
using TinYard.Extensions.MediatorMap.API.Base;
using TinYard.Extensions.ViewController.Tests.MockClasses;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class TestMediator : Mediator
    {
        [Inject]
        public TestView View;

        public event Action OnViewEventHeard;
        public event Action OnContextEventHeard;

        public override void Configure()
        {
            AddViewListener(TestEvent.Type.Test1, () => OnViewEventHeard?.Invoke());

            AddContextListener(TestEvent.Type.Test1, () => OnContextEventHeard?.Invoke());
        }
    }
}
