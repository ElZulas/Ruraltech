using RuralTech.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RuralTech.Core.DTOs;

public class CreateEventoBovinoDto
{
    [Required(ErrorMessage = "El ID del bovino es requerido")]
    public Guid BovinoId { get; set; }

    [Required(ErrorMessage = "El tipo de evento es requerido")]
    public TipoEventoBovino TipoEvento { get; set; }

    [Required(ErrorMessage = "La fecha del evento es requerida")]
    public DateTime FechaEvento { get; set; }

    [Required(ErrorMessage = "Los detalles del evento son requeridos")]
    public Dictionary<string, object> DetallesJson { get; set; } = new(); // Se serializará a JSON

    [Required(ErrorMessage = "El costo asociado es requerido")]
    [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a 0")]
    public decimal CostoAsociado { get; set; }

    public Guid? RegistradoPorUserId { get; set; } // Si lo registra el propietario
    public Guid? RegistradoPorColaboradorId { get; set; } // Si lo registra un colaborador
}
