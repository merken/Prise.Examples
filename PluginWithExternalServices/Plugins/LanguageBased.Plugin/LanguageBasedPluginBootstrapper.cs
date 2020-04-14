using Language.Domain;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    [PluginBootstrapper(PluginType = typeof(LanguageBasedPlugin))]
    public class LanguageBasedPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Register 3rd party services, not known to the MyHost
            services.AddScoped<IDictionaryService, DictionaryService>();
            return services;
        }
    }
}
