namespace RuralTech.Core.Entities;

public class Infraestructura
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UPPId { get; set; }
    public UPP UPP { get; set; } = null!;
    public string Nombre { get; set; } = string.Empty;
    public TipoInstalacion TipoInstalacion { get; set; }
    public int CapacidadMaxima { get; set; }
    public decimal? SuperficieHectareas { get; set; } // Solo para potreros
    public EstatusInfraestructura Estatus { get; set; } = EstatusInfraestructura.DISPONIBLE;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public List<Animal> Animals { get; set; } = new();
}
