using System;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandMap
    {
        ICommandMapping Map<T>() where T : IEvent;
        ICommandMapping Map<T>(Enum type) where T : IEvent;
    }
}
