using RuralTech.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RuralTech.Core.DTOs;

public class CreateInfraestructuraDto
{
    [Required(ErrorMessage = "El ID de la UPP es requerido")]
    public Guid UPPId { get; set; }

    [Required(ErrorMessage = "El nombre es requerido")]
    [StringLength(50, ErrorMessage = "El nombre no puede exceder 50 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de instalación es requerido")]
    public TipoInstalacion TipoInstalacion { get; set; }

    [Required(ErrorMessage = "La capacidad máxima es requerida")]
    [Range(1, int.MaxValue, ErrorMessage = "La capacidad máxima debe ser mayor a 0")]
    public int CapacidadMaxima { get; set; }

    [Range(0.01, 99999.99, ErrorMessage = "La superficie debe estar entre 0.01 y 99999.99 hectáreas")]
    public decimal? SuperficieHectareas { get; set; }

    public EstatusInfraestructura Estatus { get; set; } = EstatusInfraestructura.DISPONIBLE;
}
