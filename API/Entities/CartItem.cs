using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YamSoft.API.Entities;

public class CartItem
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CartId { get; set; }
    
    [ForeignKey(nameof(CartId))]
    public Cart Cart { get; set; } = null!;
    
    [Required]
    public int ProductId { get; set; }
    
    [ForeignKey(nameof(ProductId))]
    public Product Product { get; set; } = null!;
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
    
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}