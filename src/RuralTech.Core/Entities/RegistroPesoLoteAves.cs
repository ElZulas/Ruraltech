namespace RuralTech.Core.Entities;

public class RegistroPesoLoteAves
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoteAvesId { get; set; }
    public LoteAves LoteAves { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public decimal PesoPromedio { get; set; } // Peso promedio del lote en kg
    public int? MuestraTamaño { get; set; } // Cantidad de aves muestreadas
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
