using System;
using System.IO;
using Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prise;
using Prise.AssemblyScanning.Discovery;
using Products.API.Infrastructure;

namespace Products.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddHttpContextAccessor(); // Add the IHttpContextAccessor for use in the TenantAssemblySelector

            var tenantConfig = new TenantConfig();
            Configuration.Bind("TenantConfig", tenantConfig);
            services.AddSingleton(tenantConfig); // Add the tenantConfig for use in the TenantAssemblySelector

            services.AddPrise<IProductsRepository>(options => options
                .WithDefaultOptions(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins"))
                .ScanForAssemblies(composer =>
                    composer.UseDiscovery())
                // UseDiscovery() comes from the Prise.AssemblyScanning.Discovery package and 
                //  it will scan the Plugins directory recursively for implementations of IProductsRepository.
                // The TenantAssemblySelector will select which assembly plugin to load.
                .ConfigureSharedServices(sharedServices =>
                {
                    // Add the configuration for use in the plugins
                    // this way, the plugins can read their own config section from the appsettings.json
                    sharedServices.AddSingleton(Configuration);
                })
               .WithAssemblySelector<TenantAssemblySelector<IProductsRepository>>()
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
