using RuralTech.Core.Entities;

namespace RuralTech.Core.DTOs;

public class InfraestructuraDto
{
    public Guid Id { get; set; }
    public Guid UPPId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public TipoInstalacion TipoInstalacion { get; set; }
    public string TipoInstalacionNombre { get; set; } = string.Empty;
    public int CapacidadMaxima { get; set; }
    public decimal? SuperficieHectareas { get; set; }
    public EstatusInfraestructura Estatus { get; set; }
    public string EstatusNombre { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int AnimalesActuales { get; set; } // Cantidad de animales actualmente en esta infraestructura
}
