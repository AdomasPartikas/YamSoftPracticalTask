using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YamSoft.API.Dtos;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController(IDatabaseService databaseService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Route("user/{userId}")]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCartByUserId(int userId)
    {
        try
        {
            var cart = await databaseService.GetCartByUserIdAsync(userId);
            cart ??= await databaseService.CreateCartAsync(userId);

            var cartDto = mapper.Map<CartResponseDto>(cart);
            return Ok(cartDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCart(int id)
    {
        try
        {
            var cart = await databaseService.GetCartByIdAsync(id);
            if (cart == null)
                return NotFound(new { error = "Cart not found" });

            var cartDto = mapper.Map<CartResponseDto>(cart);
            return Ok(cartDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Route("user/{userId}")]
    [ProducesResponseType(typeof(CartResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateCart(int userId)
    {
        try
        {
            var existingCart = await databaseService.GetCartByUserIdAsync(userId);
            if (existingCart != null)
                return BadRequest(new { error = "User already has a cart" });

            var cart = await databaseService.CreateCartAsync(userId);
            var cartDto = mapper.Map<CartResponseDto>(cart);

            return CreatedAtAction(nameof(GetCart), new { id = cart.Id }, cartDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [Route("{cartId}/items")]
    [ProducesResponseType(typeof(CartItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToCart(int cartId, [FromBody] AddToCartDto addToCartDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cart = await databaseService.GetCartByIdAsync(cartId);
            if (cart == null)
                return NotFound(new { error = "Cart not found" });

            var product = await databaseService.GetProductByIdAsync(addToCartDto.ProductId);
            if (product == null)
                return NotFound(new { error = "Product not found" });

            if (product.Stock < addToCartDto.Quantity)
                return BadRequest(new { error = "Insufficient stock" });

            var cartItem = await databaseService.AddCartItemAsync(cartId, addToCartDto.ProductId, addToCartDto.Quantity);
            var cartItemDto = mapper.Map<CartItemResponseDto>(cartItem);

            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    [Route("items/{cartItemId}")]
    [ProducesResponseType(typeof(CartItemResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCartItem(int cartItemId, [FromBody] UpdateCartItemDto updateCartItemDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var cartItem = await databaseService.GetCartItemByIdAsync(cartItemId);

            if (cartItem == null)
                return NotFound(new { error = "Cart item not found" });

            var product = await databaseService.GetProductByIdAsync(cartItem.ProductId);
            if (product != null && product.Stock < updateCartItemDto.Quantity)
                return BadRequest(new { error = "Insufficient stock" });

            cartItem.Quantity = updateCartItemDto.Quantity;
            await databaseService.UpdateCartItemAsync(cartItem);

            var cartItemDto = mapper.Map<CartItemResponseDto>(cartItem);
            return Ok(cartItemDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    [Route("items/{cartItemId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> RemoveFromCart(int cartItemId)
    {
        try
        {
            await databaseService.RemoveCartItemAsync(cartItemId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("{cartId}/items")]
    [ProducesResponseType(typeof(IEnumerable<CartItemResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCartItems(int cartId)
    {
        try
        {
            var cart = await databaseService.GetCartByIdAsync(cartId);
            if (cart == null)
                return NotFound(new { error = "Cart not found" });

            var cartItems = await databaseService.GetCartItemsByCartIdAsync(cartId);
            var cartItemDtos = mapper.Map<IEnumerable<CartItemDto>>(cartItems);

            return Ok(cartItemDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    [Route("{cartId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCart(int cartId)
    {
        try
        {
            var cart = await databaseService.GetCartByIdAsync(cartId);
            if (cart == null)
                return NotFound(new { error = "Cart not found" });

            await databaseService.DeleteCartAsync(cartId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}