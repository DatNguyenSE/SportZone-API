using Adidas.API.Extensions;
using Adidas.Application.Dtos;
using Adidas.Application.Interfaces.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Adidas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController(ICartService cartService) : ControllerBase
    {
        [HttpPost("add")]
        public async Task<IActionResult> AddItemToCart(AddCartItemDto dto)
        {
            var userId = User.GetUserId();
            await cartService.AddItemToCartAsync(userId, dto.ProductId, dto.Quantity);
            return Ok();
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = User.GetUserId();
            await cartService.ClearCartAsync(userId);
            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.GetUserId();
            var cart =  await cartService.GetCartByUserIdAsync(userId);
            return Ok(cart);
        }

        [HttpDelete("remove/{productId:int}")]
        public async Task<IActionResult> RemoveItemFromCart(int productId)
        {
            var userId = User.GetUserId();
            await cartService.RemoveItemFromCartAsync(userId, productId);
            return NoContent();
        }
        
        [HttpPost("update")]
        public async Task<IActionResult> UpdateItemQuantity(CartItemDto dto)
        {
            var userId = User.GetUserId();
            await cartService.UpdateItemQuantityAsync(userId, dto.ProductId, dto.Quantity);
            return Ok();
        }
    }
}
