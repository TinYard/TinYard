using System;
using System.Collections.Generic;
using System.Text;

namespace TinYard.API.Interfaces
{
    public interface IConfig
    {
        object Environment { get; }

        void Configure();
    }
}
