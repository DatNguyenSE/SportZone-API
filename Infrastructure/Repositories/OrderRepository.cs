using System;
using Adidas.Application.Interfaces.IRepositories;
using Adidas.Domain.Enums;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace Adidas.Infrastructure.Repositories;

public class OrderRepository(AppDbContext _context) : GenericRepository<Order>(_context), IOrderRepository
{
    public async Task<Order?> GetOrderWithDetailsAsync(int id, string userId)
    {
        return await _context.Orders
            .Include(o => o.Payment)       
            .Include(o => o.Items)          
                .ThenInclude(i => i.Product) 
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);
    }

    public async Task<Order?> GetOrderWithPaymentAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.Payment) // Join bảng Payment
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)          
            .ThenInclude(i => i.Product)    
            .OrderByDescending(o => o.CreatedAt) 
            .ToListAsync();
    }

    public async Task<bool> CancelOrderAsync(int orderId, string userId)
    {
        var order = await _context.Orders
            .Include(o => o.Payment) // include payment to set status = failed
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        if (order == null)
        {
            return false;
        }
        order.Status = OrderStatus.Cancelled;
        order.Payment!.PaymentStatus = PaymentStatus.Failed; 
        return true;
    }
}
