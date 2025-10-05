using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YamSoft.API.Entities;

public class Cart
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    
    [NotMapped]
    public decimal TotalAmount => CartItems.Sum(ci => ci.Quantity * ci.Product.Price);
}