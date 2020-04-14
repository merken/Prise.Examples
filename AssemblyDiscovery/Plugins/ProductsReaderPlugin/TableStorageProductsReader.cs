using Contract;
using Prise.Plugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TableStorageConnector;

namespace ProductsReaderPlugin
{
    [Plugin(PluginType = typeof(IProductsReader))]
    public class TableStorageProductsReader : TableStorageConnector<Product>, IProductsReader
    {
        /// <summary>
        /// Because we use inheritance (TableStorageConnector base class) we cannot rely on the Prise Field Injection, so a PluginFactory is still required.
        /// </summary>
        /// <param name="config"></param>
        internal TableStorageProductsReader(
            TableStorageConfig config)
            : base(config,
                  (p) => p.Id.ToString(),
                  (p, value) => p.Id = int.Parse(value),
                  (p) => p.Name,
                  (p, value) => p.Name = value)
        {
        }

        /// <summary>
        /// This PluginFactory method will be called when the plugin is activated (instantiated)
        /// At this point, you can retrieve any service that was registered using the PluginBootstrapper linked to this plugin
        /// You could also retrieve any shared or host service that was registered in the Startup.cs file of the host application
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        [PluginFactory]
        public static TableStorageProductsReader ThisIsTheFactoryMethod(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(TableStorageConfig)) as TableStorageConfig;
            return new TableStorageProductsReader(config);
        }

        public async Task<IEnumerable<Product>> All()
        {
            return (await base.GetAll()).Select(e => e.Value);
        }

        public async Task<Product> Get(int productId)
        {
            var items = await base.Search($"id={productId}");
            return items.Select(e => e.Value).FirstOrDefault();
        }

        public async Task<IEnumerable<Product>> Search(string term)
        {
            return (await base.Search(term)).Select(e => e.Value);
        }
    }
}
