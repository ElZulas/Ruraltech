using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RuralTech.Core.DTOs;
using RuralTech.Core.Entities;
using RuralTech.Infrastructure.Data;
using System.Security.Claims;

namespace RuralTech.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] string? category, [FromQuery] bool? featured)
    {
        var query = _context.Products
            .Include(p => p.Seller)
            .Where(p => p.IsActive);

        if (!string.IsNullOrEmpty(category))
        {
            query = query.Where(p => p.Category == category);
        }

        if (featured == true)
        {
            query = query.Where(p => p.IsFeatured);
        }

        var products = await query.ToListAsync();

        return Ok(products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price,
            Category = p.Category,
            ImageUrl = p.ImageUrl,
            SellerName = p.Seller.FullName,
            Location = p.Location ?? p.Seller.Location,
            Phone = p.Phone ?? p.Seller.Phone,
            WhatsApp = p.WhatsApp,
            Rating = p.Rating,
            ReviewCount = p.ReviewCount,
            IsFeatured = p.IsFeatured
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductDto>> GetProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Seller)
            .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            ImageUrl = product.ImageUrl,
            SellerName = product.Seller.FullName,
            Location = product.Location ?? product.Seller.Location,
            Phone = product.Phone ?? product.Seller.Phone,
            WhatsApp = product.WhatsApp,
            Rating = product.Rating,
            ReviewCount = product.ReviewCount,
            IsFeatured = product.IsFeatured
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductDto dto)
    {
        var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Category = dto.Category,
            ImageUrl = dto.ImageUrl,
            SellerId = userId,
            Location = dto.Location,
            Phone = dto.Phone,
            WhatsApp = dto.WhatsApp,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var savedProduct = await _context.Products
            .Include(p => p.Seller)
            .FirstAsync(p => p.Id == product.Id);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, new ProductDto
        {
            Id = savedProduct.Id,
            Name = savedProduct.Name,
            Description = savedProduct.Description,
            Price = savedProduct.Price,
            Category = savedProduct.Category,
            ImageUrl = savedProduct.ImageUrl,
            SellerName = savedProduct.Seller.FullName,
            Location = savedProduct.Location ?? savedProduct.Seller.Location,
            Phone = savedProduct.Phone ?? savedProduct.Seller.Phone,
            WhatsApp = savedProduct.WhatsApp
        });
    }

    [HttpGet("categories")]
    public async Task<ActionResult<List<string>>> GetCategories()
    {
        var categories = await _context.Products
            .Where(p => p.IsActive)
            .Select(p => p.Category)
            .Distinct()
            .ToListAsync();

        return Ok(categories);
    }
}
