using System;
using TinYard.Framework.API.Interfaces;

namespace TinYard.Extensions.CommandSystem.API.Interfaces
{
    public interface ICommandFactory : IFactory
    {
        ICommand Build<T>() where T : ICommand;
        ICommand Build(Type commandType);
    }
}
