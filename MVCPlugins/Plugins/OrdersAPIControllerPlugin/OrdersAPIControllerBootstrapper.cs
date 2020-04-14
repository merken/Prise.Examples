using Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace OrdersAPIControllerPlugin
{
    [PluginBootstrapper(PluginType = typeof(OrdersAPIController))]
    public class OrdersAPIControllerBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            services.AddScoped<OrdersConfig>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                var ordersConfig = new OrdersConfig();
                config.Bind("Orders", ordersConfig);

                return ordersConfig;
            });

            return services;
        }
    }
}
