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
            bool recurringTimer = false;

            switch (evt.type)
            {
                case AddCallbackTimerEvent.Type.AddRecurring:

                    recurringTimer = true;

                    break;
            }

            if (evt.SecondsDuration >= 0d)
            {
                if(recurringTimer)
                    callbackTimer.AddRecurringTimer(evt.SecondsDuration, callback);
                else
                    callbackTimer.AddTimer(evt.SecondsDuration, callback);
            }
            else if (evt.TicksDuration >= 0)
            {
                if(recurringTimer)
                    callbackTimer.AddRecurringTimer(evt.TicksDuration, callback);
                else
                    callbackTimer.AddTimer(evt.TicksDuration, callback);
            }
        }
    }
}
