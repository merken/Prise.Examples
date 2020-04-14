using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrdersAPIControllerPlugin
{
    public class TableStorageProvider<T> where T : TableEntity, new()
    {
        private readonly OrdersConfig config;
        private CloudTable table;

        internal TableStorageProvider(OrdersConfig config)
        {
            this.config = config;
        }

        public async Task ConnectToTableAsync()
        {
            CloudStorageAccount storageAccount = new CloudStorageAccount(
                new StorageCredentials(this.config.StorageAccount, this.config.StorageKey), false);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            this.table = tableClient.GetTableReference(this.config.TableName);
            await table.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            TableQuery<T> query = new TableQuery<T>();

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<IEnumerable<T>> Search(string term)
        {
            TableQuery<T> query = new TableQuery<T>();
            query.FilterString = term;

            List<T> results = new List<T>();
            TableContinuationToken continuationToken = null;
            do
            {
                TableQuerySegment<T> queryResults =
                    await table.ExecuteQuerySegmentedAsync(query, continuationToken);

                continuationToken = queryResults.ContinuationToken;
                results.AddRange(queryResults.Results);

            } while (continuationToken != null);

            return results;
        }

        public async Task<T> InsertOrUpdate(T entity)
        {
            var operation = TableOperation.InsertOrReplace(entity);
            await this.table.ExecuteAsync(operation);
            return entity;
        }

        public async Task<T> GetItem(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(operation);

            return (T)(dynamic)result.Result;
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            var item = await GetItem(partitionKey, rowKey);
            var operation = TableOperation.Delete(item);
            await this.table.ExecuteAsync(operation);
        }
    }
}
