namespace YamSoft.API.Dtos;

public class CartItemDto
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public ProductResponseDto? Product { get; set; }
    public int Quantity { get; set; }
    public DateTime AddedAt { get; set; }
}

public class CartItemResponseDto : CartItemDto
{
    public int Id { get; set; }
}

public class UpdateCartItemDto : CartItemDto
{
    // Can be extended in the future if needed
}