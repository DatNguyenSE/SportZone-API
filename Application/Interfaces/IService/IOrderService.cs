using System;
using Adidas.Application.Dtos;
using API.Entities;

namespace Adidas.Application.Interfaces.IService;

public interface IOrderService
{
    Task<OrderDto?> GetOrderWithPaymentAsync(int id);
    Task<OrderDto?> GetOrderWithDetailsAsync(int orderId, string userId);
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
    Task<int> CreateOrderByCartItemsAsync(string userId,string paymentMethod);
    Task CancelOrderAsync(int orderId, string userId);
}
