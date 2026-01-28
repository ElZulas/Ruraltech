using RuralTech.Core.Entities;

namespace RuralTech.Core.DTOs;

public class LoteAvesDto
{
    public Guid Id { get; set; }
    public Guid UPPId { get; set; }
    public Guid InfraestructuraId { get; set; }
    public string InfraestructuraNombre { get; set; } = string.Empty;
    
    // Identificación
    public string CodigoLote { get; set; } = string.Empty;
    public string? NumeroLoteGranja { get; set; }
    
    // Información
    public DateTime FechaIngreso { get; set; }
    public int CantidadInicial { get; set; }
    public int CantidadActual { get; set; }
    public int MortalidadTotal { get; set; } // Calculado: CantidadInicial - CantidadActual
    public decimal? MortalidadPorcentaje { get; set; } // Calculado
    public string Raza { get; set; } = string.Empty;
    public string? TipoAve { get; set; }
    public int? EdadDias { get; set; }
    public int? EdadActualDias { get; set; } // Calculado desde FechaIngreso
    
    // Estado
    public EstatusLoteAves Estatus { get; set; }
    public string EstatusNombre { get; set; } = string.Empty;
    public DateTime? FechaFinalizacion { get; set; }
    
    // Estadísticas
    public decimal? PesoPromedioActual { get; set; } // Último registro de peso
    public int TotalVacunaciones { get; set; }
    public int TotalTratamientos { get; set; }
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
