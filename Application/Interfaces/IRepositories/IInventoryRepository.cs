using System;
using API.Entities;

namespace Adidas.Application.Interfaces.IRepositories;

public interface IInventoryRepository : IGenericRepository<Inventory>
{
    Task<int> GetQuantityAsync(int productID);
    Task<IEnumerable<Inventory>> GetListByProductIdsAsync(IEnumerable<int> productIDs);
}
