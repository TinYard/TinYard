using System;

namespace TinYard.Extensions.CallbackTimer.Impl.VO
{
    public class Timer
    {
        public Action TimerCallback { get; protected set; }

        public double TimerDuration { get; protected set; }

        public double CurrentLifetime { get; protected set; }

        protected bool _timerFinished = false;

        public Timer(double durationInSeconds, Action timerCallback)
        {
            TimerDuration = durationInSeconds;
            CurrentLifetime = 0d;

            TimerCallback = timerCallback;
        }

        public virtual void Update(double deltaTime)
        {
            CurrentLifetime += deltaTime;

            if (!_timerFinished && CurrentLifetime >= TimerDuration)
            {
                _timerFinished = true;

                TimerCallback?.Invoke();
            }
        }
    }
}
