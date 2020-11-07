using System;
using System.Collections.Generic;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Base;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandMapping
    {
        Type Event { get; }
        Enum EventType { get; }

        Type Command { get; }

        IReadOnlyList<Type> GuardTypes { get; }

        ICommandMapping Map<T>(Enum type = null) where T : IEvent;

        ICommandMapping ToCommand<T>() where T : ICommand;

        ICommandMapping WithGuard<T>() where T : Guard;
        ICommandMapping WithGuard<T1, T2>() where T1 : Guard where T2 : Guard;
        ICommandMapping WithGuard<T1, T2, T3>() where T1 : Guard where T2 : Guard where T3 : Guard;
        ICommandMapping WithGuard<T1, T2, T3, T4>() where T1 : Guard where T2 : Guard where T3 : Guard where T4 : Guard;
        ICommandMapping WithGuard<T1, T2, T3, T4, T5>() where T1 : Guard where T2 : Guard where T3 : Guard where T4 : Guard where T5 : Guard;
    }
}
