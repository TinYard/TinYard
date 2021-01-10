using System;
using System.Collections.Generic;
using System.Text;
using TinYard.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Tests.TestClasses
{
    public class DependableTestConfig : IConfig
    {
        [Inject]
        public IContext context;

        public void Configure()
        {
            context.Mapper.Map<int>().ToValue(69);
        }
    }
}
