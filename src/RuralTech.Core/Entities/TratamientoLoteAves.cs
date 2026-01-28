namespace RuralTech.Core.Entities;

public class TratamientoLoteAves
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoteAvesId { get; set; }
    public LoteAves LoteAves { get; set; } = null!;
    public string Condicion { get; set; } = string.Empty; // Enfermedad o condición tratada
    public string DescripcionTratamiento { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string? Medicamento { get; set; }
    public string? Dosis { get; set; }
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
