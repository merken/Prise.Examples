using Contract;
using Prise;
using Prise.Infrastructure;
using System;
using System.Net.Http;

namespace Plugin.Function.Infrastructure
{
    public class FunctionPluginLoaderOptions
    {
        private readonly IPluginLoadOptions<IHelloPlugin> helloPluginLoadOptions;
        private readonly IPluginLogger<IHelloPlugin> pluginLogger;
        private readonly IPluginPathProvider<IHelloPlugin> pluginPathProvider;
        private readonly IHostTypesProvider<IHelloPlugin> hostTypesProvider;
        private readonly IRuntimePlatformContext runtimePlatformContext;
        private readonly IHostFrameworkProvider hostFrameworkProvider;
        private readonly IDependencyPathProvider<IHelloPlugin> dependencyPathProvider;
        private readonly IProbingPathsProvider<IHelloPlugin> probingPathsProvider;
        private readonly IPluginDependencyResolver<IHelloPlugin> pluginDependencyResolver;
        private readonly INativeAssemblyUnloader nativeAssemblyUnloader;
        private readonly IRemoteTypesProvider<IHelloPlugin> remoteTypesProvider;
        private readonly ITempPathProvider<IHelloPlugin> tempPathProvider;
        private readonly IAssemblyLoadStrategyProvider assemblyLoadStrategyProvider;
        private readonly IPluginServerOptions pluginServerOptions;
        private readonly IHttpClientFactory httpFactory;

        public FunctionPluginLoaderOptions(
            IPluginLoadOptions<IHelloPlugin> helloPluginLoadOptions,
            IPluginLogger<IHelloPlugin> pluginLogger,
            IPluginPathProvider<IHelloPlugin> pluginPathProvider,
            IHostTypesProvider<IHelloPlugin> hostTypesProvider,
            IRemoteTypesProvider<IHelloPlugin> remoteTypesProvider,
            IRuntimePlatformContext runtimePlatformContext,
            IHostFrameworkProvider hostFrameworkProvider,
            IDependencyPathProvider<IHelloPlugin> dependencyPathProvider,
            IProbingPathsProvider<IHelloPlugin> probingPathsProvider,
            IPluginDependencyResolver<IHelloPlugin> pluginDependencyResolver,
            INativeAssemblyUnloader nativeAssemblyUnloader,
            ITempPathProvider<IHelloPlugin> tempPathProvider,
            IAssemblyLoadStrategyProvider assemblyLoadStrategyProvider,
            IPluginServerOptions pluginServerOptions,
            IHttpClientFactory httpFactory)
        {
            this.helloPluginLoadOptions = helloPluginLoadOptions;
            this.pluginLogger = pluginLogger;
            this.pluginPathProvider = pluginPathProvider;
            this.hostTypesProvider = hostTypesProvider;
            this.remoteTypesProvider = remoteTypesProvider;
            this.runtimePlatformContext = runtimePlatformContext;
            this.hostFrameworkProvider = hostFrameworkProvider;
            this.dependencyPathProvider = dependencyPathProvider;
            this.probingPathsProvider = probingPathsProvider;
            this.pluginDependencyResolver = pluginDependencyResolver;
            this.nativeAssemblyUnloader = nativeAssemblyUnloader;
            this.tempPathProvider = tempPathProvider;
            this.assemblyLoadStrategyProvider = assemblyLoadStrategyProvider;
            this.pluginServerOptions = pluginServerOptions;
            this.httpFactory = httpFactory;
        }

        public IPluginLoader<IHelloPlugin> CreateLoaderForComponent(string functionComponent)
        {
            var networkAssemblyLoaderOptions = new DefaultNetworkAssemblyLoaderOptions<IHelloPlugin>(
                                $"{this.pluginServerOptions.PluginServerUrl}/{functionComponent}",
                                ignorePlatformInconsistencies: true); // The plugins are netstandard, so we must ignore inconsistencies

            var depsFileProvider = new NetworkDepsFileProvider<IHelloPlugin>(networkAssemblyLoaderOptions, this.httpFactory);

            var networkAssemblyLoader = new NetworkAssemblyLoader<IHelloPlugin>(
                    this.pluginLogger,
                    networkAssemblyLoaderOptions,
                    this.hostFrameworkProvider,
                    this.hostTypesProvider,
                    this.remoteTypesProvider,
                    this.dependencyPathProvider,
                    this.probingPathsProvider,
                    this.runtimePlatformContext,
                    depsFileProvider,
                    this.pluginDependencyResolver,
                    this.nativeAssemblyUnloader,
                    this.assemblyLoadStrategyProvider,
                    this.tempPathProvider,
                    this.httpFactory);

            var defaultScannerOptions = new DefaultAssemblyScannerOptions<IHelloPlugin>(new DefaultPluginPathProvider<IHelloPlugin>(String.Empty), this.helloPluginLoadOptions.RuntimePlatformContext);

            var loaderOptions = new PluginLoadOptions<IHelloPlugin>(
                this.pluginLogger,
                new DefaultAssemblyScanner<IHelloPlugin>(defaultScannerOptions, new PluginAssemblyNameProvider<IHelloPlugin>($"{functionComponent}.dll")),
                this.helloPluginLoadOptions.SharedServicesProvider,
                this.helloPluginLoadOptions.PluginTypesProvider,
                this.helloPluginLoadOptions.PluginActivationContextProvider,
                this.helloPluginLoadOptions.Activator,
                this.helloPluginLoadOptions.ParameterConverter,
                this.helloPluginLoadOptions.ResultConverter,
                networkAssemblyLoader,
                this.helloPluginLoadOptions.ProxyCreator,
                this.helloPluginLoadOptions.HostTypesProvider,
                this.helloPluginLoadOptions.RemoteTypesProvider,
                this.helloPluginLoadOptions.RuntimePlatformContext,
                this.helloPluginLoadOptions.AssemblySelector,
                this.helloPluginLoadOptions.PluginSelector
            );

            return new PrisePluginLoader<IHelloPlugin>(loaderOptions);
        }
    }
}