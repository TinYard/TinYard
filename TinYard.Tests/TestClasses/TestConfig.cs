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

        public void Configure()
        {
            
        }
    }
}
