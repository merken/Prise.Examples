using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Logging;
using MyHost.Models;
using Prise;
using Prise.AssemblyScanning;
using Prise.Infrastructure;
using Prise.Mvc;
using Prise.Mvc.Infrastructure;

namespace MyHost.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationPartManager applicationPartManager;
        private readonly ILogger<HomeController> logger;
        private readonly IAssemblyScanner<IMVCFeature> assemblyScanner;
        private readonly IPluginAssemblyLoader<IMVCFeature> pluginAssemblyLoader;
        private readonly IPriseActionDescriptorChangeProvider pluginChangeProvider;
        private readonly IPluginCache<IMVCFeature> pluginCache;

        public HomeController(
            ApplicationPartManager applicationPartManager,
            ILogger<HomeController> logger,
            IAssemblyScanner<IMVCFeature> assemblyScanner,
            IPluginAssemblyLoader<IMVCFeature> pluginAssemblyLoader,
            IPriseActionDescriptorChangeProvider pluginChangeProvider,
            IPluginCache<IMVCFeature> pluginCache
        )
        {
            this.applicationPartManager = applicationPartManager;
            this.logger = logger;
            this.assemblyScanner = assemblyScanner;
            this.pluginAssemblyLoader = pluginAssemblyLoader;
            this.pluginChangeProvider = pluginChangeProvider;
            this.pluginCache = pluginCache;
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetHomeViewModel());
        }

        private async Task<HomeViewModel> GetHomeViewModel()
        {
            var pluginAssemblies = await this.assemblyScanner.Scan();
            var applicationParts = this.applicationPartManager.ApplicationParts;

            var loadedPlugins = from plugin in pluginAssemblies
                                let pluginName = Path.GetFileNameWithoutExtension(plugin.AssemblyName)
                                let pluginType = plugin.PluginType
                                let pluginDescriptionAttribute = CustomAttributeData.GetCustomAttributes(pluginType).FirstOrDefault(c => c.AttributeType.Name == typeof(MVCFeatureDescriptionAttribute).Name)
                                let pluginDescription = pluginDescriptionAttribute.NamedArguments.FirstOrDefault(a => a.MemberName == "Description").TypedValue.Value as string
                                join part in applicationParts
                                    on pluginName equals part.Name
                                    into pluginParts
                                from pluginPart in pluginParts.DefaultIfEmpty()
                                select new Feature
                                {
                                    Name = pluginName,
                                    Description = pluginDescription,
                                    IsEnabled = pluginPart != null
                                };

            return new HomeViewModel() { Features = loadedPlugins };
        }

        [Route("home/enable/{pluginName}")]
        public async Task<IActionResult> Enable(string pluginName)
        {
            if (String.IsNullOrEmpty(pluginName))
            {
                return NotFound();
            }

            var pluginAssemblies = await this.assemblyScanner.Scan();
            var pluginToEnable = pluginAssemblies.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p.AssemblyName) == pluginName);
            if (pluginToEnable == null)
                return NotFound();

            var assemblyPluginLoadContext = DefaultPluginLoadContext<IMVCFeature>.FromAssemblyScanResult(pluginToEnable);
            var pluginAssembly = await this.pluginAssemblyLoader.LoadAsync(assemblyPluginLoadContext);
            var feature = pluginAssembly.GetTypes().FirstOrDefault(t => t.GetInterfaces().Any(i => i.Name == typeof(IMVCFeature).Name));
            var featureDescriptionAttribute = feature.GetCustomAttributes(false).FirstOrDefault(c => c.GetType().Name == typeof(MVCFeatureDescriptionAttribute).Name);
            var description = featureDescriptionAttribute.GetType().GetProperty("Description").GetValue(featureDescriptionAttribute);
            this.applicationPartManager.ApplicationParts.Add(new PluginAssemblyPart(pluginAssembly));
            this.pluginCache.Add(pluginAssembly);
            this.pluginChangeProvider.TriggerPluginChanged();

            return Redirect("/");
        }

        [Route("home/disable/{pluginName}")]
        public async Task<IActionResult> Disable(string pluginName)
        {
            if (String.IsNullOrEmpty(pluginName))
            {
                return NotFound();
            }

            var pluginAssemblies = await this.assemblyScanner.Scan();
            var pluginToDisable = pluginAssemblies.FirstOrDefault(p => Path.GetFileNameWithoutExtension(p.AssemblyName) == pluginName);
            if (pluginToDisable == null)
                return NotFound();

            var pluginAssemblyToDisable = Path.GetFileNameWithoutExtension(pluginToDisable.AssemblyName);
            var partToRemove = this.applicationPartManager.ApplicationParts.FirstOrDefault(a => a.Name == pluginAssemblyToDisable);

            this.applicationPartManager.ApplicationParts.Remove(partToRemove);
            await this.pluginAssemblyLoader.UnloadAsync(pluginAssemblyToDisable);
            this.pluginCache.Remove(pluginAssemblyToDisable);
            this.pluginChangeProvider.TriggerPluginChanged();

            return Redirect("/");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
