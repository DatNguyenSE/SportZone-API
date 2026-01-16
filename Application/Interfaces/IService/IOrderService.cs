using System;
using SportZone.Application.Dtos;
using SportZone.Domain.Enums;
using API.Entities;

namespace SportZone.Application.Interfaces.IService;

public interface IOrderService
{
    Task<IEnumerable<OrderDetailsDto>> GetOrderWithPaymentAsync(string userId, PaymentStatus paymentStatus);
    Task<OrderDetailsDto?> GetOrderWithDetailsAsync(int orderId, string userId);
    Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId);
    Task<OrderDetailsDto> CreateOrderByCartItemsAsync(string userId, PaymentMethod paymentMethod);
    Task CancelOrderAsync(int orderId, string userId);
}
