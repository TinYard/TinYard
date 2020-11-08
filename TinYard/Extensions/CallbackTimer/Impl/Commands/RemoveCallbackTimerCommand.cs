using TinYard.Extensions.CallbackTimer.API.Events;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.CallbackTimer.Impl.Commands
{
    public class RemoveCallbackTimerCommand : ICommand
    {
        [Inject]
        public RemoveCallbackTimerEvent evt;

        [Inject]
        public ICallbackTimer callbackTimer;

        public void Execute()
        {
            callbackTimer.RemoveTimer(evt.CallbackToRemove);
        }
    }
}
