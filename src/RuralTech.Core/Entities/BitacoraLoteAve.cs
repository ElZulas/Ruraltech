namespace RuralTech.Core.Entities;

public class BitacoraLoteAve
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LoteAvesId { get; set; }
    public LoteAves LoteAves { get; set; } = null!;
    public DateTime FechaRegistro { get; set; } // Día del reporte
    public int DiaCiclo { get; set; } // Edad en días del lote (calculado: FechaRegistro - FechaIngreso del lote)
    public int Mortalidad { get; set; } = 0; // Cantidad de aves muertas hoy (default 0)
    public int Descarte { get; set; } = 0; // Aves vivas sacadas por enfermedad (default 0)
    public decimal ConsumoKg { get; set; } // Kilos de alimento servidos (vital para Conversión Alimenticia)
    public decimal? PesoPromedio { get; set; } // Muestreo de peso (opcional, se pesan ~10 pollos y se anota el promedio)
    public string? Observaciones { get; set; } // Notas del operario (opcional)
    
    // Timestamps
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
