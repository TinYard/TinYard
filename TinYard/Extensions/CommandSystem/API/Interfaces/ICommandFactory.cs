using System;
using TinYard.Extensions.EventSystem.API.Interfaces;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandFactory : IFactory
    {
        T Build<T>(IEvent evtToInject = null) where T : ICommand;
        ICommand Build(Type commandType, IEvent evtToInject = null);
    }
}
