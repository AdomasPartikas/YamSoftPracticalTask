namespace YamSoft.API.Dtos;

public class CartDto
{
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public decimal TotalAmount { get; set; }
    public ICollection<CartItemDto> CartItems { get; set; } = [];

}

public class AddToCartDto : CartDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}