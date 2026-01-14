using Adidas.API.Extensions;
using Adidas.Application.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adidas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> CreateOrder(string paymentMethod)
        {
            var userId = User.GetUserId();
            var orderId = await orderService.CreateOrderByCartItemsAsync(userId, paymentMethod);
            return CreatedAtAction(nameof(GetOrderWithDetails), new { orderId = orderId }, orderId);
        }

        [HttpGet("user-orders")]
        public async Task<IActionResult> GetOrdersByUserId()
        {
            var userId = User.GetUserId();
            var orders = await orderService.GetOrdersByUserIdAsync(userId);
            return Ok(orders);
        }

        [HttpGet("{orderId:int}/details")]
        public async Task<IActionResult> GetOrderWithDetails(int orderId)
        {
            var userId = User.GetUserId();
            var order = await orderService.GetOrderWithDetailsAsync(orderId, userId);
            return Ok(order);
        }

        [HttpDelete("{orderId:int}/cancel")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var userId = User.GetUserId();
            await orderService.CancelOrderAsync(orderId, userId);
            return Ok(new { Message = "Order cancelled successfully." });
        }
    }
}
