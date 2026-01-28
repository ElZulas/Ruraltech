using RuralTech.Core.Entities;

namespace RuralTech.Core.DTOs;

public class BovinoDto
{
    public Guid Id { get; set; }
    public Guid UPPId { get; set; }
    public string? InfraestructuraId { get; set; }
    public string? InfraestructuraNombre { get; set; }
    
    // Identificadores
    public string? AreteSiniiga { get; set; }
    public string AreteTrabajo { get; set; } = string.Empty; // Requerido
    
    // Datos básicos
    public string? Nombre { get; set; } // Opcional
    public DateTime FechaNacimiento { get; set; } // Requerido
    public int? EdadMeses { get; set; } // Calculado
    public char Sexo { get; set; } // Requerido
    public string SexoNombre { get; set; } = string.Empty; // "Macho" o "Hembra"
    public string RazaPredominante { get; set; } = string.Empty; // Requerido
    
    // Relaciones parentales
    public Guid? MadreId { get; set; } // Opcional
    public string? MadreNombre { get; set; }
    public Guid? PadreId { get; set; }
    public string? PadreNombre { get; set; }
    
    // Estado y estatus
    public EstadoProductivo EstadoProductivo { get; set; }
    public string EstadoProductivoNombre { get; set; } = string.Empty;
    public EstatusSistema EstatusSistema { get; set; }
    public string EstatusSistemaNombre { get; set; } = string.Empty;
    
    // Timestamps
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
