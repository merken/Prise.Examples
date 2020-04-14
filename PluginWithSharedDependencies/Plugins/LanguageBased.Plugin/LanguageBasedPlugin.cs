using Contract;
using Language.Domain;
using Prise.Plugin;
using System;

namespace LanguageBased.Plugin
{
    [Plugin(PluginType = typeof(IHelloPlugin))]
    public class LanguageBasedPlugin : IHelloPlugin
    {
        /// <summary>
        /// This dependency is registered via the PluginBootstrapper
        /// </summary>
        [PluginService(ServiceType = typeof(ILanguageService), ProvidedBy = ProvidedBy.Plugin)]
        private readonly ILanguageService languageService;

        /// <summary>
        /// This dependency is registered at the MyHost host app
        /// </summary>
        [PluginService(ServiceType = typeof(ISharedLanguageService), ProvidedBy = ProvidedBy.Host, BridgeType = typeof(SharedLanguageServiceBridge))]
        private readonly ISharedLanguageService sharedLanguageService;

        public string SayHello(string input)
        {
            if (this.sharedLanguageService == null)
                throw new Exception("sharedLanguageService is null");
            if (this.languageService == null)
                throw new Exception("languageService is null");
            var language = this.sharedLanguageService.GetLanguage();
            var dictionary = this.languageService.GetLanguageDictionary();

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }
    }
}
