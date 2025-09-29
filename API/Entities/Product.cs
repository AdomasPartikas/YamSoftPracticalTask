using System.ComponentModel.DataAnnotations;

namespace YamSoft.API.Entities;

public class Product
{
    [Key]
    public int Id { get; set; }
    [Required]
    public required string Name { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int Stock { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
}