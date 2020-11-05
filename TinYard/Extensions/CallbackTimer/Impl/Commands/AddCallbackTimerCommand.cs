using System;
using TinYard.Extensions.CallbackTimer.API.Events;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CommandSystem.API.Interfaces;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.CallbackTimer.Impl.Commands
{
    public class AddCallbackTimerCommand : ICommand
    {
        [Inject]
        public AddCallbackTimerEvent evt;

        [Inject]
        public ICallbackTimer callbackTimer;

        public void Execute()
        {
            Action callback = evt.OnFinishedCallback;

            if(evt.SecondsDuration >= 0d)
            {
                callbackTimer.AddTimer(evt.SecondsDuration, callback);
            }
            else if(evt.TicksDuration >= 0)
            {
                callbackTimer.AddTimer(evt.TicksDuration, callback);
            }
        }
    }
}
