using System;
using System.IO;
using Contract;
using ExternalServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prise;

namespace MyHost
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
            services.AddHttpContextAccessor();
            services.AddScoped<IExternalService, AcceptHeaderlanguageService>();

            services.AddPrise<IHelloPlugin>(options => options
                .WithDefaultOptions(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins", "LanguageBased.Plugin"))
                .WithPluginAssemblyName("LanguageBased.Plugin.dll")
                .IgnorePlatformInconsistencies() // The plugin is a netstandard library, the host is a netcoreapp, ignore this inconsistency
                .ConfigureHostServices(hostServices =>
                {
                    // These services are registered as host types
                    // Their types and instances will be loaded from the MyHost
                    hostServices.AddHttpContextAccessor();
                    hostServices.AddSingleton<IConfiguration>(Configuration);
                })
                .ConfigureSharedServices(sharedServices =>
                {
                    // The AcceptHeaderlanguageService is known in the MyHost, but the type is registered as a remote type
                    // This encourages backwards compatability
                    sharedServices.AddTransient<IExternalService, AcceptHeaderlanguageService>();
                })
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
