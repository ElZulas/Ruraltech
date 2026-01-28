using System.ComponentModel.DataAnnotations;

namespace RuralTech.Core.DTOs;

public class CreateBitacoraLoteAveDto
{
    [Required(ErrorMessage = "El ID del lote es requerido")]
    public Guid LoteAvesId { get; set; }

    [Required(ErrorMessage = "La fecha del registro es requerida")]
    public DateTime FechaRegistro { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "La mortalidad debe ser mayor o igual a 0")]
    public int Mortalidad { get; set; } = 0;

    [Range(0, int.MaxValue, ErrorMessage = "El descarte debe ser mayor o igual a 0")]
    public int Descarte { get; set; } = 0;

    [Required(ErrorMessage = "El consumo de alimento es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El consumo debe ser mayor o igual a 0")]
    public decimal ConsumoKg { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "El peso promedio debe ser mayor o igual a 0")]
    public decimal? PesoPromedio { get; set; }

    [StringLength(2000, ErrorMessage = "Las observaciones no pueden exceder 2000 caracteres")]
    public string? Observaciones { get; set; }
}
