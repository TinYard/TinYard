using System;
using System.Collections.Generic;
using System.Text;
using TinYard.Extensions.EventSystem.Impl.Base;

namespace TinYard.Extensions.CallbackTimer.API.Events
{
    public class AddCallbackTimerEvent : Event
    {
        public enum Type
        {
            Add,
            AddRecurring
        }

        public int TicksDuration { get; private set; } = -1;
        public double SecondsDuration { get; private set; } = -1d;

        public Action OnFinishedCallback { get; private set; }

        public AddCallbackTimerEvent(Type type, int tickDuration, Action onFinishedCallback) : base(type)
        {
            TicksDuration = tickDuration;
            OnFinishedCallback = onFinishedCallback;
        }

        public AddCallbackTimerEvent(Type type, double secondsDuration, Action onFinishedCallback) : base(type)
        {
            SecondsDuration = secondsDuration;
            OnFinishedCallback = onFinishedCallback;
        }
    }
}
