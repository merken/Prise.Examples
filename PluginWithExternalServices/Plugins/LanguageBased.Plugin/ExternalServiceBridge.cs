using System;
using System.Threading.Tasks;
using Contract;
using ExternalServices;
using Prise.PluginBridge;

namespace LanguageBased.Plugin
{
    public class ExternalServiceBridge : IExternalService
    {
        private readonly object hostService;
        public ExternalServiceBridge(object hostService)
        {
            this.hostService = hostService;
        }

        public ExternalObject GetExternalObject()
        {
            var methodInfo = typeof(IExternalService).GetMethod(nameof(GetExternalObject));
            return PrisePluginBridge.Invoke(this.hostService, methodInfo) as ExternalObject;
        }

        public Task<ExternalObject> GetExternalObjectAsync()
        {
            var methodInfo = typeof(IExternalService).GetMethod(nameof(GetExternalObjectAsync));
            return PrisePluginBridge.Invoke(this.hostService, methodInfo) as Task<ExternalObject>;
        }

        public string GetLanguage()
        {
            var methodInfo = typeof(IExternalService).GetMethod(nameof(GetLanguage));
            var result = PrisePluginBridge.Invoke(this.hostService, methodInfo);
            return result as string;
        }

        public Task<ExternalObject> ModifyExternalObject(ExternalObject external)
        {
            var methodInfo = typeof(IExternalService).GetMethod(nameof(ModifyExternalObject));
            return PrisePluginBridge.Invoke(this.hostService, methodInfo, external) as Task<ExternalObject>;
        }
    }
}
