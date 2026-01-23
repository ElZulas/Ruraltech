namespace RuralTech.Core.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string SellerName { get; set; } = string.Empty;
    public string? Location { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsFeatured { get; set; }
}

public class CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Location { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
}
