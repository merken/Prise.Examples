using Contract;
using Prise.PluginBridge;

namespace LanguageBased.Plugin
{
    public class SharedLanguageServiceBridge : ISharedLanguageService
    {
        private readonly object hostService;
        public SharedLanguageServiceBridge(object hostService)
        {
            this.hostService = hostService;
        }

        public string GetLanguage()
        {
            var methodInfo = typeof(ISharedLanguageService).GetMethod(nameof(GetLanguage));
            return PrisePluginBridge.Invoke(this.hostService, methodInfo) as string;
        }
    }
}
