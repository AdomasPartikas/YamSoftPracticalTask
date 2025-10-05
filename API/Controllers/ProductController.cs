using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using YamSoft.API.Dtos;
using YamSoft.API.Entities;
using YamSoft.API.Interfaces;

namespace YamSoft.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IDatabaseService databaseService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await databaseService.GetAllProductsAsync();
            var productDtos = mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProduct(int id)
    {
        try
        {
            var product = await databaseService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { error = "Product not found" });

            var productDto = mapper.Map<ProductResponseDto>(product);
            return Ok(productDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpGet]
    [Route("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> SearchProducts([FromQuery] string name)
    {
        try
        {
            var products = await databaseService.GetProductsByNameAsync(name);
            var productDtos = mapper.Map<IEnumerable<ProductResponseDto>>(products);
            return Ok(productDtos);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductDto createProductDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = mapper.Map<Product>(createProductDto);
            var createdProduct = await databaseService.CreateProductAsync(product);
            var productDto = mapper.Map<ProductResponseDto>(createdProduct);

            return CreatedAtAction(nameof(GetProduct), new { id = createdProduct.Id }, productDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDto updateProductDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = await databaseService.GetProductByIdAsync(id);
            if (existingProduct == null)
                return NotFound(new { error = "Product not found" });

            mapper.Map(updateProductDto, existingProduct);

            await databaseService.UpdateProductAsync(existingProduct);
            var productDto = mapper.Map<ProductResponseDto>(existingProduct);

            return Ok(productDto);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        try
        {
            var product = await databaseService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound(new { error = "Product not found" });

            await databaseService.DeleteProductAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}