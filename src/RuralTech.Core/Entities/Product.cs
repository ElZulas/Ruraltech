namespace RuralTech.Core.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty; // "Ganado", "Medicinas", "Herramientas"
    public string? ImageUrl { get; set; }
    public Guid SellerId { get; set; }
    public User Seller { get; set; } = null!;
    public string? Location { get; set; }
    public string? Phone { get; set; }
    public string? WhatsApp { get; set; }
    public decimal? Rating { get; set; }
    public int ReviewCount { get; set; }
    public bool IsFeatured { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
