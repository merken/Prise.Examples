using System;
using System.Linq;
using System.Threading.Tasks;
using Contract;
using Prise.Plugin;
using TableStorageConnector;

namespace ProductsDeleterPlugin
{
    [Plugin(PluginType = typeof(IProductsDeleter))]
    public class TableStorageProductsDeleter : TableStorageConnector<Product>, IProductsDeleter
    {
        internal TableStorageProductsDeleter(
            TableStorageConfig config)
            : base(config,
                  (p) => p.Id.ToString(),
                  (p, value) => p.Id = int.Parse(value),
                  (p) => p.Name,
                  (p, value) => p.Name = value)
        {
        }

        [PluginFactory]
        public static TableStorageProductsDeleter ThisIsTheFactoryMethod(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(TableStorageConfig)) as TableStorageConfig;
            return new TableStorageProductsDeleter(config);
        }

        public async Task Delete(int productId)
        {
            var items = await base.Search($"Id eq {productId}");
            var item = items.First();
            await base.Delete(item.PartitionKey, item.RowKey);
        }
    }
}
