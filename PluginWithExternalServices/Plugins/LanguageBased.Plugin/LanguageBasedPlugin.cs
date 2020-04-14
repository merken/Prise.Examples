using System;
using System.Threading.Tasks;
using Contract;
using ExternalServices;
using Language.Domain;
using Microsoft.Extensions.Configuration;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    [Plugin(PluginType = typeof(IHelloPlugin))]
    public class LanguageBasedPlugin : IHelloPlugin
    {
        [PluginService(ServiceType = typeof(IConfiguration), ProvidedBy = ProvidedBy.Host)]
        private readonly IConfiguration configuration;

        [PluginService(ServiceType = typeof(IExternalService), ProvidedBy = ProvidedBy.Host, BridgeType = typeof(ExternalServiceBridge))]
        private readonly IExternalService externalService;

        [PluginService(ServiceType = typeof(IDictionaryService), ProvidedBy = ProvidedBy.Plugin)]
        private readonly IDictionaryService dictionaryService;

        public string SayHello(string input)
        {
            var language = this.externalService.GetExternalObject().Language;
            var dictionary = dictionaryService.GetLanguageDictionary().Result;

            var languageFromConfig = this.configuration["LanuageOverride"];
            if (!String.IsNullOrEmpty(languageFromConfig))
                language = languageFromConfig;

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }

        public async Task<string> SayHelloAsync(string input)
        {
            var language = (await this.externalService.GetExternalObjectAsync()).Language;
            var dictionary = await dictionaryService.GetLanguageDictionary();

            var languageFromConfig = this.configuration["LanuageOverride"];
            if (!String.IsNullOrEmpty(languageFromConfig))
                language = languageFromConfig;

            if (dictionary.ContainsKey(language))
                return $"{dictionary[language]} {input}";

            return $"We could not find a suitable word for language {language}";
        }
    }
}
