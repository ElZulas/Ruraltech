using RuralTech.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RuralTech.Core.DTOs;

public class CreateBovinoDto
{
    [Required(ErrorMessage = "El ID de la UPP es requerido")]
    public Guid UPPId { get; set; }

    public Guid? InfraestructuraId { get; set; }

    [StringLength(14, ErrorMessage = "El arete SINIIGA debe tener máximo 14 caracteres")]
    [RegularExpression(@"^MX00[A-Z]{2}\d{8}$", ErrorMessage = "Formato inválido. Debe ser: MX + 00 + Estado (2 letras) + 8 dígitos consecutivos")]
    public string? AreteSiniiga { get; set; }

    [Required(ErrorMessage = "El arete de trabajo es requerido")]
    [StringLength(10, ErrorMessage = "El arete de trabajo no puede exceder 10 caracteres")]
    public string AreteTrabajo { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
    public DateTime FechaNacimiento { get; set; }

    [Required(ErrorMessage = "El sexo es requerido")]
    [RegularExpression(@"^[MH]$", ErrorMessage = "El sexo debe ser M (Macho) o H (Hembra)")]
    public char Sexo { get; set; }

    [Required(ErrorMessage = "La raza predominante es requerida")]
    [StringLength(50, ErrorMessage = "La raza predominante no puede exceder 50 caracteres")]
    public string RazaPredominante { get; set; } = string.Empty;

    public Guid? MadreId { get; set; }

    public Guid? PadreId { get; set; }

    [Required(ErrorMessage = "El estado productivo es requerido")]
    public EstadoProductivo EstadoProductivo { get; set; }

    [Required(ErrorMessage = "El estatus del sistema es requerido")]
    public EstatusSistema EstatusSistema { get; set; } = EstatusSistema.ACTIVO;
}
