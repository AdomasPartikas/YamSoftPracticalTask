namespace YamSoft.API.Dtos;

public class ProductDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public string? ImageUrl { get; set; }
}

public class CreateProductDto : ProductDto
{
    // Inherits all properties from ProductDto
}

public class UpdateProductDto : ProductDto
{
    // Inherits all properties from ProductDto
}