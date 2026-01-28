namespace RuralTech.Core.Entities;

public class Bovino
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UPPId { get; set; }
    public UPP UPP { get; set; } = null!;
    public Guid? InfraestructuraId { get; set; }
    public Infraestructura? Infraestructura { get; set; }
    
    // Identificadores
    public string? AreteSiniiga { get; set; } // VARCHAR(14) - Formato: MX + 00 + XX + 00000000 (opcional al nacer, obligatorio para ventas)
    public string AreteTrabajo { get; set; } = string.Empty; // VARCHAR(10) - Número visual corto (Chapa) - REQUERIDO
    
    // Datos básicos
    public string? Nombre { get; set; } // VARCHAR(50) - Apodo del animal (opcional)
    public DateTime FechaNacimiento { get; set; } // DATE - Fecha real o estimada - REQUERIDO
    public char Sexo { get; set; } // CHAR(1) - M (Macho) o H (Hembra), inmutable - REQUERIDO
    public string RazaPredominante { get; set; } = string.Empty; // VARCHAR(50) - Viene de Catálogo - REQUERIDO
    
    // Relaciones parentales (recursivas)
    public Guid? MadreId { get; set; } // Optional - FK recursiva
    public Bovino? Madre { get; set; }
    public Guid? PadreId { get; set; } // Optional - FK recursiva
    public Bovino? Padre { get; set; }
    
    // Estado y estatus
    public EstadoProductivo EstadoProductivo { get; set; } // Required
    public EstatusSistema EstatusSistema { get; set; } // Required
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones con registros históricos (mantener compatibilidad con sistema existente)
    public List<WeightRecord> WeightHistory { get; set; } = new();
    public List<Vaccine> Vaccines { get; set; } = new();
    public List<Treatment> Treatments { get; set; } = new();
    public List<EventoBovino> Eventos { get; set; } = new(); // Historial clínico inmutable
}
