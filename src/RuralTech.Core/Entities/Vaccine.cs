namespace RuralTech.Core.Entities;

public class Vaccine
{
    public int Id { get; set; }
    public int AnimalId { get; set; }
    public Animal Animal { get; set; } = null!;
    public string Name { get; set; } = string.Empty;
    public DateTime DateApplied { get; set; }
    public DateTime NextDueDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
