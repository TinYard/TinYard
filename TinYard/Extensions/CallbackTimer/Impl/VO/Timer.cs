using System;

namespace TinYard.Extensions.CallbackTimer.Impl.VO
{
    public class Timer
    {
        public Action TimerCallback { get { return _timerCallback; } }
        private Action _timerCallback;

        public double TimerDuration { get { return _timerDuration; } }
        private double _timerDuration;

        public double CurrentLifetime { get { return _currentLifetime; } }
        private double _currentLifetime;

        private bool _timerFinished = false;

        public Timer(double durationInSeconds, Action timerCallback)
        {
            _timerDuration = durationInSeconds;
            _currentLifetime = 0d;

            _timerCallback = timerCallback;
        }

        public void Update(double deltaTime)
        {
            _currentLifetime += deltaTime;

            if (!_timerFinished && _currentLifetime >= TimerDuration)
            {
                _timerFinished = true;

                _timerCallback?.Invoke();
            }
        }
    }
}
