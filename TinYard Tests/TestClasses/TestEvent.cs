using TinYard.Extensions.EventSystem.Impl.Base;

namespace TinYard.Extensions.EventSystem.Tests.MockClasses
{
    public class TestEvent : Event
    {
        public enum Type
        {
            Test1,
            Test2,
            Test3
        }

        public TestEvent(Type type) : base(type)
        {

        }
    }
}
