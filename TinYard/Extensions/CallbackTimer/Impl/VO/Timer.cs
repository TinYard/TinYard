using System;

namespace TinYard.Extensions.CallbackTimer.Impl.VO
{
    public class Timer
    {
        public Action TimerCallback { get; private set; }

        public double TimerDuration { get; private set; }

        public double CurrentLifetime { get; private set; }

        private bool _timerFinished = false;

        public Timer(double durationInSeconds, Action timerCallback)
        {
            TimerDuration = durationInSeconds;
            CurrentLifetime = 0d;

            TimerCallback = timerCallback;
        }

        public void Update(double deltaTime)
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
