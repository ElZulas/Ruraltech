namespace RuralTech.Core.Entities;

public class VacunacionLoteAves
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoteAvesId { get; set; }
    public LoteAves LoteAves { get; set; } = null!;
    public string NombreVacuna { get; set; } = string.Empty;
    public DateTime FechaAplicacion { get; set; }
    public DateTime? FechaProximaAplicacion { get; set; }
    public int? CantidadAplicada { get; set; } // Cantidad de aves vacunadas
    public string? MetodoAplicacion { get; set; } // Agua, spray, inyección, etc.
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
