using System;
using TinYard.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.MockClasses
{
    public class TestConfig : IConfig
    {
        [Inject]
        public IContext context;

        public object Environment { get { return _environment; } }
        private object _environment;

        public readonly bool HaveDependencies;

        public TestConfig(bool haveDependencies = false)
        {
            HaveDependencies = haveDependencies;
        }

        public void Configure()
        {
            if(HaveDependencies)
            {
                var val = context.Mapper.GetMappingValue<int>();

                if (val == 0)
                    throw new ArgumentOutOfRangeException(nameof(val), "Value not mapped.");
            }
        }
    }
}
