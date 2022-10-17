﻿using TinYard.API.Interfaces;
using TinYard.Extensions.CallbackTimer.API.Events;
using TinYard.Extensions.CallbackTimer.API.Services;
using TinYard.Extensions.CallbackTimer.Impl.Commands;
using TinYard.Extensions.CallbackTimer.Impl.Services;
using TinYard.Extensions.CommandSystem.API.Interfaces;

namespace TinYard.Extensions.CallbackTimer
{
    public class CallbackTimerExtension : IExtension
    {
        public object Environment { get { return _environment; } }
        private object _environment;

        public CallbackTimerExtension(object environment = null)
        {
            _environment = environment;
        }

        public void Install(IContext context)
        {
            var callbackTimerService = new CallbackTimerService();
            callbackTimerService.Startup();

            context.Mapper.Map<ICallbackTimer>().ToSingleton(callbackTimerService);

            ICommandMap commandMap = context.Mapper.GetMappingValue<ICommandMap>();

            commandMap.Map<AddCallbackTimerEvent>(AddCallbackTimerEvent.Type.Add).ToCommand<AddCallbackTimerCommand>();
            commandMap.Map<AddCallbackTimerEvent>(AddCallbackTimerEvent.Type.AddRecurring).ToCommand<AddCallbackTimerCommand>();
            commandMap.Map<RemoveCallbackTimerEvent>(RemoveCallbackTimerEvent.Type.Remove).ToCommand<RemoveCallbackTimerCommand>();
        }
    }
}
