using Language.Domain;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace LanguageBased.Plugin
{
    /// <summary>
    /// This bootstrapper will be used in order to load the LanguageBasedPlugin
    /// </summary>
    [PluginBootstrapper(PluginType = typeof(LanguageBasedPlugin))]
    public class LanguageBasedPluginBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            // Register all services that are required to load the LanguageBasedPlugin
            services.AddScoped<ILanguageService, LanguageService>();
            return services;
        }
    }
}
