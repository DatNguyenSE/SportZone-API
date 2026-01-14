using System;
using Adidas.Domain.Enums;
using API.Entities;

namespace Adidas.Application.Interfaces.IRepositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order?>> GetOrderWithPaymentAsync(string userId, PaymentStatus paymentStatus);
    Task<Order?> GetOrderWithDetailsAsync(int OrderId, string userId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

}
