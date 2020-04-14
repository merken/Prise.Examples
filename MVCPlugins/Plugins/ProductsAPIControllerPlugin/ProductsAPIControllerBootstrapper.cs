using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prise.Plugin;

namespace ProductsAPIControllerPlugin
{
    [PluginBootstrapper(PluginType = typeof(ProductsAPIController))]
    public class ProductsAPIControllerBootstrapper : IPluginBootstrapper
    {
        public IServiceCollection Bootstrap(IServiceCollection services)
        {
            services.AddScoped<DbConnection>((serviceProvider) =>
            {
                var config = serviceProvider.GetRequiredService<IConfiguration>();
                // using Microsoft.Data.SqlClient
                var dbConnection = new SqlConnection(config.GetConnectionString("Products"));
                dbConnection.Open();
                return dbConnection;
            });

            services.AddScoped<DbContextOptions>((serviceProvider) =>
            {
                var dbConnection = serviceProvider.GetService<DbConnection>();
                return new DbContextOptionsBuilder<ProductsDbContext>()
                    .UseSqlServer(dbConnection)
                    .Options;
            });

            services.AddScoped<ProductsDbContext>((serviceProvider) =>
            {
                var options = serviceProvider.GetService<DbContextOptions>();
                var context = new ProductsDbContext(options);
                return context;
            });

            return services;
        }
    }
}
