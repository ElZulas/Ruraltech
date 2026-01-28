using RuralTech.Core.Entities;
using System.Text.Json;

namespace RuralTech.Core.DTOs;

public class EventoBovinoDto
{
    public Guid Id { get; set; }
    public Guid BovinoId { get; set; }
    public string? BovinoNombre { get; set; }
    public string? BovinoAreteTrabajo { get; set; }
    public TipoEventoBovino TipoEvento { get; set; }
    public string TipoEventoNombre { get; set; } = string.Empty;
    public DateTime FechaEvento { get; set; }
    public Dictionary<string, object> DetallesJson { get; set; } = new(); // Deserializado del JSON
    public decimal CostoAsociado { get; set; }
    public Guid? RegistradoPorUserId { get; set; }
    public string? RegistradoPorUserName { get; set; }
    public Guid? RegistradoPorColaboradorId { get; set; }
    public string? RegistradoPorColaboradorNombre { get; set; }
    public DateTime CreatedAt { get; set; } // Fecha de registro en el sistema
}
