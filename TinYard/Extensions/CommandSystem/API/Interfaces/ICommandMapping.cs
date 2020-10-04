using System;
using System.Collections.Generic;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandMapping
    {
        Type Event { get; }
        Enum EventType { get; }

        Type Command { get; }

        IReadOnlyList<IGuard> Guards { get; }

        ICommandMapping Map<T>(Enum type = null) where T : IEvent;

        ICommandMapping ToCommand<T>() where T : ICommand;

        ICommandMapping WithGuard<T>() where T : IGuard;
    }
}
