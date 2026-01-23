namespace RuralTech.Core.Entities;

public class UPP
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OwnerId { get; set; } // id_propietario
    public User Owner { get; set; } = null!;
    public string ClavePGN { get; set; } = string.Empty; // clave_pgn
    public string NombrePredio { get; set; } = string.Empty; // nombre_predio
    public string PropietarioLegal { get; set; } = string.Empty; // propietario_legal
    public string CodigoQRAcceso { get; set; } = string.Empty; // codigo_qr_acceso
    public string EstadoMX { get; set; } = string.Empty; // estado_mx (CHAR(2))
    public decimal? Latitude { get; set; } // coordenadas - latitud
    public decimal? Longitude { get; set; } // coordenadas - longitud
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Relaciones
    public List<Animal> Animals { get; set; } = new();
    public List<Colaborador> Colaboradores { get; set; } = new();
}
