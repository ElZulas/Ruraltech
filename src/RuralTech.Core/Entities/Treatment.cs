namespace RuralTech.Core.Entities;

public class Treatment
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public string Condition { get; set; } = string.Empty;
    public string TreatmentDescription { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
