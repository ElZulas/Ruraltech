namespace RuralTech.Core.DTOs;

public class BitacoraLoteAveDto
{
    public Guid Id { get; set; }
    public Guid LoteAvesId { get; set; }
    public string? LoteCodigo { get; set; }
    public DateTime FechaRegistro { get; set; }
    public int DiaCiclo { get; set; } // Edad en días del lote
    public int Mortalidad { get; set; }
    public int Descarte { get; set; }
    public decimal ConsumoKg { get; set; }
    public decimal? PesoPromedio { get; set; }
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Campos calculados para análisis
    public decimal? ConversionAlimenticia { get; set; } // ConsumoKg / (CantidadActual * PesoPromedio) si ambos están disponibles
}
