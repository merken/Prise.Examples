using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Prise.Plugin;
using ProductsAPIControllerPlugin.Models;

namespace ProductsAPIControllerPlugin
{
    [ApiController]
    [Route("api/products")]
    public class ProductsAPIController : ControllerBase
    {
        private readonly ProductsDbContext dbContext;
        internal ProductsAPIController(ProductsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [PluginFactory]
        public static ProductsAPIController CreateInstanceOfController(IServiceProvider serviceProvider)
        {
            var service = serviceProvider.GetService(typeof(ProductsDbContext));
            return new ProductsAPIController(service as ProductsDbContext);
        }

        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            return await dbContext.Products
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
