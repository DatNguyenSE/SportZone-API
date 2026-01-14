using System;
using API.Entities;

namespace Adidas.Application.Interfaces.IRepositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<Order?> GetOrderWithPaymentAsync(int id);
    Task<Order?> GetOrderWithDetailsAsync(int OrderId, string userId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    Task<bool> CancelOrderAsync(int orderId,string userId);

}
