using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.VO;
using TinYard.Framework.Impl.Attributes;

namespace TinYard.Extensions.CallbackTimer.Impl.Services
{
    public class CallbackTimerService : ICallbackTimer
    {
        private Task _updateTask;

        private List<Timer> _timers = new List<Timer>();

        public CallbackTimerService()
        {
            _timers = new List<Timer>();

            _updateTask = Task.Run(Update);
        }

        private void Update()
        {
            double deltaTime = 0d;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while(true)
            {
                UpdateTimers(deltaTime);

                deltaTime = stopwatch.Elapsed.TotalSeconds;

                //TODO : Look at if we can improve performance here by not using restart and instead tracking previous elapsed time
                stopwatch.Restart();
            }
        }

        /// <summary>
        /// This is public for testing purposes only. 
        /// Do not call this method unless you know what you are doing!
        /// </summary>
        /// <param name="deltaTime">Time interval since last update</param>
        public void UpdateTimers(double deltaTime)
        {
            lock (_timers)
            {
                foreach(Timer timer in _timers)
                {
                    timer.Update(deltaTime);
                }

                _timers.RemoveAll(timer => timer.CurrentLifetime >= timer.TimerDuration);
            }
        }

        public void AddTimer(int ticks, Action callback)
        {
            double ticksInSeconds = GetDurationOfTicksInSeconds(ticks);
            CreateTimer(ticksInSeconds, callback);
        }

        public void AddTimer(double seconds, Action callback)
        {
            CreateTimer(seconds, callback);
        }

        public bool RemoveTimer(Action callbackToRemove)
        {
            int timersRemoved = 0;
            lock(_timers)
            {
                timersRemoved = _timers.RemoveAll(timer => timer.TimerCallback == callbackToRemove);
            }

            return timersRemoved > 0;
        }

        private void CreateTimer(double durationInSeconds, Action callback)
        {
            Timer timer = new Timer(durationInSeconds, callback);

            lock (_timers)
                _timers.Add(timer);
        }

        private double GetDurationOfTicksInSeconds(int ticks)
        {
            //Number of ticks into seconds
            return Stopwatch.Frequency * ticks;
        }
    }
}
