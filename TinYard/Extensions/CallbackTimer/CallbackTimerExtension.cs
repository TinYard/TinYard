using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Services;

namespace TinYard.Extensions.CallbackTimer
{
    public class CallbackTimerExtension : IExtension
    {
        public void Install(IContext context)
        {
            context.Mapper.Map<ICallbackTimer>().ToValue(new CallbackTimerService());
        }
    }
}
