using System;
using SportZone.Domain.Enums;
using API.Entities;

namespace SportZone.Application.Interfaces.IRepositories;

public interface IOrderRepository : IGenericRepository<Order>
{
    Task<IEnumerable<Order?>> GetOrderWithPaymentAsync(string userId, PaymentStatus paymentStatus);
    Task<Order?> GetOrderWithDetailsAsync(int OrderId, string userId);
    Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

}
