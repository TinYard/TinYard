using System;
using TinYard.Extensions.EventSystem.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandMapping
    {
        Type Event { get; }
        Enum EventType { get; }

        Type Command { get; }

        ICommandMapping Map<T>(Enum type = null) where T : IEvent;

        ICommandMapping ToCommand<T>() where T : ICommand;
    }
}
