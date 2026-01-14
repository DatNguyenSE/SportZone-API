using System;
using Adidas.Application.Dtos;
using Adidas.Application.Interfaces;
using Adidas.Application.Interfaces.IService;
using Adidas.Domain.Enums;
using Adidas.Domain.Exceptions;
using API.Entities;
using AutoMapper;

namespace Adidas.Application.Services;

public class OrderService(IUnitOfWork uow, IMapper mapper) : IOrderService
{
    public async Task<int> CreateOrderByCartItemsAsync(string userId, string paymentMethod)
    {
        // 1. get user cart
        var userCart = await uow.CartRepository.GetCartByUserIdAsync(userId);

        if (userCart == null || userCart.Items.Count == 0)
        {
            throw new InvalidOperationException("Cart is empty or does not exist.");
        }
        // 2.1 get productId list from cart
        var productIds = userCart.Items.Select(i => i.ProductId).ToList();
        // 2.2 get list inventories for list products in cart
        var inventories = await uow.InventoryRepository.GetListByProductIdsAsync(productIds);

        // 3. create order object
        var order = new Order
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };

        decimal totalAmount = 0;

        foreach (var cartItem in userCart.Items)
        {
            if (cartItem.Product == null)
            {
                throw new Exception($"Product is null for CartItem ID: {cartItem.CartId}");
            }
            // 4. find corresponding inventory
            var inventory = inventories.FirstOrDefault(x => x.ProductId == cartItem.ProductId);
            if (inventory == null)
            {
                throw new NotFoundException($"Inventory not found for Product ID: {cartItem.ProductId}");
            }

            if (inventory.Quantity < cartItem.Quantity)
            {
                throw new InvalidOperationException($"Not enough stock for Product '{cartItem.Product.Name}'. Available: {inventory.Quantity}, Requested: {cartItem.Quantity}");
            }

            inventory.Quantity -= cartItem.Quantity;
            inventory.UpdatedAt = DateTime.UtcNow;

            var orderItem = new OrderItem
            {
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.Product.Price
            };

            totalAmount += orderItem.Quantity * orderItem.UnitPrice;
            order.Items.Add(orderItem); // add eachh order item to order
        }

        order.TotalAmount = totalAmount;

        // Pay by COD
        if (paymentMethod == "COD")
        {
            order.Payment = new Payment
            {
                    PaymentMethod = PaymentMethod.COD,
                    PaymentStatus = PaymentStatus.Pending,
                PaidAt = DateTime.UtcNow
            };

            order.Status = OrderStatus.Placed;
        }
        // Pay by Online

        //     Client gọi tiếp API thanh toán -> Thành công.

        // Update DB: Order (Status: Paid), Payment (Status: Success, TransactionId: ...).
        // Map Order DTO sang Entity

        await uow.OrderRepository.AddAsync(order);
        await uow.CartRepository.ClearCartAsync(userId);

        // Commit tất cả thay đổi (Order, Inventory, Cart) cùng lúc
        await uow.Complete();

        return order.Id;
    }

    public async Task<IEnumerable<OrderDto>> GetOrdersByUserIdAsync(string userId)
    {
        var orders = await uow.OrderRepository.GetOrdersByUserIdAsync(userId);
        return mapper.Map<IEnumerable<OrderDto>>(orders);
    }

    public async Task<OrderDto?> GetOrderWithDetailsAsync(int orderId, string userId)
    {
        var orderEntity = await uow.OrderRepository.GetOrderWithDetailsAsync(orderId, userId);
        if(orderEntity == null)
        {
            throw new NotFoundException($"Order with ID {orderId} not found.");
        }
        return mapper.Map<OrderDto>(orderEntity);
    }

    public Task<OrderDto?> GetOrderWithPaymentAsync(int id)
    {
        throw new NotImplementedException();
    }
    public async Task CancelOrderAsync(int orderId, string userId)
    {
        //kiem tra xem thanh cong thi khong cancel duoc
        var result = await uow.OrderRepository.CancelOrderAsync(orderId, userId);
        if (!result)
        {
            throw new NotFoundException($"Order with ID {orderId} not found.");
        }
        await uow.Complete();
    }
}
