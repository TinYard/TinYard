using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Events;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Commands;
using TinYard.Extensions.CallbackTimer.Impl.Services;
using TinYard.Extensions.CommandSystem.API.Interfaces;

namespace TinYard.Extensions.CallbackTimer
{
    public class CallbackTimerExtension : IExtension
    {
        public void Install(IContext context)
        {
            context.Mapper.Map<ICallbackTimer>().ToValue(new CallbackTimerService());

            ICommandMap commandMap = context.Mapper.GetMappingValue<ICommandMap>();

            commandMap.Map<AddCallbackTimerEvent>(AddCallbackTimerEvent.Type.Add).ToCommand<AddCallbackTimerCommand>();
            commandMap.Map<RemoveCallbackTimerEvent>(RemoveCallbackTimerEvent.Type.Remove).ToCommand<RemoveCallbackTimerCommand>();
        }
    }
}
