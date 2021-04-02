using System;

namespace TinYard.Extensions.CallbackTimer.Impl.VO
{
    public class RecurringTimer : Timer
    {
        public RecurringTimer(double durationInSeconds, Action timerCallback) : base(durationInSeconds, timerCallback)
        {
        }

        public override void Update(double deltaTime)
        {
            base.Update(deltaTime);

            if(_timerFinished)
            {
                RestartTimer();
            }
        }

        private void RestartTimer()
        {
            CurrentLifetime = 0d;
            _timerFinished = false;
        }
    }
}
