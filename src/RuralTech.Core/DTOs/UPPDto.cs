namespace RuralTech.Core.DTOs;

public class UPPDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string ClavePGN { get; set; } = string.Empty;
    public string NombrePredio { get; set; } = string.Empty;
    public string PropietarioLegal { get; set; } = string.Empty;
    public string CodigoQRAcceso { get; set; } = string.Empty;
    public string EstadoMX { get; set; } = string.Empty;
    public decimal? Latitude { get; set; }
    public decimal? Longitude { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int AnimalCount { get; set; }
}
