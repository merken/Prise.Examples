using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contract
{
    /// <summary>
    /// OldSQLPlugin only implements All() and Get(int productId).
    /// SQLPlugin and TableStoragePlugin implement the contract described below.
    /// 
    /// Although OldSQLPlugin implements and older version of the Contract.dll assembly, it can still be loaded and invoked by Prise via the PriseProxy!
    /// </summary>
    public interface IProductsRepository
    {
        Task<IEnumerable<Product>> All();
        Task<Product> Get(int productId);
        Task<Product> Create(Product product);
        Task<Product> Update(Product product);
        Task Delete(int productId);
        Task<IEnumerable<Product>> Search(string term);
    }
}
