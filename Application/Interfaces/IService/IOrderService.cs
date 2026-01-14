using System;
using Adidas.Application.Dtos;
using Adidas.Domain.Enums;
using API.Entities;

namespace Adidas.Application.Interfaces.IService;

public interface IOrderService
{
    Task<IEnumerable<OrderDetailsDto>> GetOrderWithPaymentAsync(string userId, PaymentStatus paymentStatus);
    Task<OrderDetailsDto?> GetOrderWithDetailsAsync(int orderId, string userId);
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
    Task<int> CreateOrderByCartItemsAsync(string userId, PaymentMethod paymentMethod);
    Task CancelOrderAsync(int orderId, string userId);
}
