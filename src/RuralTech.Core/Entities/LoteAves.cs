namespace RuralTech.Core.Entities;

public class LoteAves
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UPPId { get; set; }
    public UPP UPP { get; set; } = null!;
    public Guid InfraestructuraId { get; set; } // Debe ser NAVE_AVICOLA
    public Infraestructura Infraestructura { get; set; } = null!;
    
    // Identificación del lote
    public string CodigoLote { get; set; } = string.Empty; // Código único del lote (ej: "LOTE-2025-001")
    public string? NumeroLoteGranja { get; set; } // Número de lote según la granja
    
    // Información del lote
    public DateTime FechaIngreso { get; set; } // Fecha de llegada de las aves
    public int CantidadInicial { get; set; } // Cantidad de aves al ingresar
    public int CantidadActual { get; set; } // Cantidad actual (se actualiza con mortalidad)
    public string Raza { get; set; } = string.Empty; // Raza o línea genética
    public string? TipoAve { get; set; } // Pollo de engorda, ponedora, reproductora, etc.
    public int? EdadDias { get; set; } // Edad en días al momento del ingreso
    
    // Estado del lote
    public EstatusLoteAves Estatus { get; set; } = EstatusLoteAves.ACTIVO;
    public DateTime? FechaFinalizacion { get; set; } // Fecha de finalización del lote
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones con registros históricos
    public List<RegistroMortalidadAves> RegistrosMortalidad { get; set; } = new();
    public List<RegistroPesoLoteAves> RegistrosPeso { get; set; } = new();
    public List<VacunacionLoteAves> Vacunaciones { get; set; } = new();
    public List<TratamientoLoteAves> Tratamientos { get; set; } = new();
    public List<BitacoraLoteAve> Bitacoras { get; set; } = new(); // Registros diarios de la bitácora
}
