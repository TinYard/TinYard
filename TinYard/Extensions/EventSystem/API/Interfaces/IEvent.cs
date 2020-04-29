using System;

namespace TinYard.Extensions.EventSystem.API.Interfaces
{
    public interface IEvent
    {
        Enum type { get; }
    }
}
