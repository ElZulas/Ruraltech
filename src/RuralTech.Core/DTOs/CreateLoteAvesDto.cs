using RuralTech.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RuralTech.Core.DTOs;

public class CreateLoteAvesDto
{
    [Required(ErrorMessage = "El ID de la UPP es requerido")]
    public Guid UPPId { get; set; }

    [Required(ErrorMessage = "El ID de la infraestructura es requerido")]
    public Guid InfraestructuraId { get; set; }

    [Required(ErrorMessage = "El código del lote es requerido")]
    [StringLength(50, ErrorMessage = "El código del lote no puede exceder 50 caracteres")]
    public string CodigoLote { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "El número de lote de granja no puede exceder 50 caracteres")]
    public string? NumeroLoteGranja { get; set; }

    [Required(ErrorMessage = "La fecha de ingreso es requerida")]
    public DateTime FechaIngreso { get; set; }

    [Required(ErrorMessage = "La cantidad inicial es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La cantidad inicial debe ser mayor a 0")]
    public int CantidadInicial { get; set; }

    [Required(ErrorMessage = "La raza es requerida")]
    [StringLength(50, ErrorMessage = "La raza no puede exceder 50 caracteres")]
    public string Raza { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "El tipo de ave no puede exceder 50 caracteres")]
    public string? TipoAve { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La edad en días debe ser mayor o igual a 0")]
    public int? EdadDias { get; set; }

    public EstatusLoteAves Estatus { get; set; } = EstatusLoteAves.ACTIVO;
}
