using System;
using System.Threading.Tasks;
using Contract;
using Prise.Plugin;
using TableStorageConnector;

namespace ProductsWriterPlugin
{
    [Plugin(PluginType = typeof(IProductsWriter))]
    public class TableStorageProductsWriter : TableStorageConnector<Product>, IProductsWriter
    {
        internal TableStorageProductsWriter(
            TableStorageConfig config)
            : base(config,
                  (p) => p.Id.ToString(),
                  (p, value) => p.Id = int.Parse(value),
                  (p) => p.Name,
                  (p, value) => p.Name = value)
        {
        }

        [PluginFactory]
        public static TableStorageProductsWriter ThisIsTheFactoryMethod(IServiceProvider serviceProvider)
        {
            var config = serviceProvider.GetService(typeof(TableStorageConfig)) as TableStorageConfig;
            return new TableStorageProductsWriter(config);
        }

        public async Task<Product> Create(Product product)
        {
            return (await base.InsertOrUpdate(product)).Value;
        }


        public async Task<Product> Update(Product product)
        {
            return (await base.InsertOrUpdate(product)).Value;
        }
    }
}
