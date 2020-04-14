using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OrdersControllerPlugin.Models;
using Prise.Plugin;

namespace OrdersAPIControllerPlugin
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersAPIController : ControllerBase
    {
        private readonly TableStorageProvider<OrderTableEntity> tableStorageProvider;

        public OrdersAPIController(OrdersConfig config)
        {
            this.tableStorageProvider = new TableStorageProvider<OrderTableEntity>(config);
        }

        [PluginFactory]
        public static OrdersAPIController CreateInstanceOfController(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(OrdersConfig));
            return new OrdersAPIController(config as OrdersConfig);
        }

        [HttpGet]
        public async Task<IEnumerable<Order>> Get()
        {
            await this.tableStorageProvider.ConnectToTableAsync();
            var tableEntities = await this.tableStorageProvider.GetAll();
            return tableEntities.Select(t => ToOrder(t));
        }

        private Order ToOrder(OrderTableEntity e) => new Order
        {
            Id = e.Id,
            Price = e.Price,
            PriceIncVAT = e.PriceIncVAT
        };
    }
}
