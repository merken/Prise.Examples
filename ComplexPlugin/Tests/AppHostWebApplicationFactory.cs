using AppHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests
{
    public class CommandLineArgumentsLazy : ICommandLineArguments
    {
        public bool UseLazyService => true;
    }

    public partial class AppHostWebApplicationFactory
       : WebApplicationFactory<AppHost.Startup>
    {
        private bool useLazyServices = false;
        public void ConfigureLazyService()
        {
            this.useLazyServices = true;
        }
    }
}