namespace RuralTech.Core.Entities;

public class WeightRecord
{
    public int Id { get; set; }
    public int? AnimalId { get; set; } // Opcional - para animales genéricos
    public Animal? Animal { get; set; }
    public Guid? BovinoId { get; set; } // Opcional - para bovinos
    public Bovino? Bovino { get; set; }
    public decimal Weight { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
