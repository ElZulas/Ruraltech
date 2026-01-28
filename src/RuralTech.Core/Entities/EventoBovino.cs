using System.Text.Json;

namespace RuralTech.Core.Entities;

public class EventoBovino
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BovinoId { get; set; }
    public Bovino Bovino { get; set; } = null!;
    public TipoEventoBovino TipoEvento { get; set; }
    public DateTime FechaEvento { get; set; } // Fecha real del evento (puede ser diferente a CreatedAt)
    public string DetallesJson { get; set; } = string.Empty; // JSONB - Datos específicos del evento (almacenado como string JSON)
    public decimal CostoAsociado { get; set; } // Gasto realizado (Medicina/Veterinario) - REQUERIDO
    public Guid? RegistradoPorUserId { get; set; } // Usuario que registró (opcional, para auditoría)
    public User? RegistradoPorUser { get; set; } // Relación con User (si es propietario)
    public Guid? RegistradoPorColaboradorId { get; set; } // Colaborador que registró (opcional, para auditoría)
    public Colaborador? RegistradoPorColaborador { get; set; } // Relación con Colaborador (si es staff)
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Fecha de registro en el sistema
}
