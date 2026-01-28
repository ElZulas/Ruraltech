namespace RuralTech.Core.Entities;

public class RegistroMortalidadAves
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoteAvesId { get; set; }
    public LoteAves LoteAves { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public int CantidadMuertas { get; set; }
    public string? CausaMuerte { get; set; } // Opcional - causa de la mortalidad
    public string? Observaciones { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
