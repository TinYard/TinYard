using System;

namespace TinYard.Extensions.CallbackTimer.API.Services
{
    public interface ICallbackTimer
    {
        void AddTimer(int ticks, Action callback);
        void AddTimer(double seconds, Action callback);

        bool RemoveTimer(Action callback);
    }
}
