namespace RuralTech.Core.Entities;

public class Animal
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Sex { get; set; } = string.Empty; // "Macho" o "Hembra"
    public decimal CurrentWeight { get; set; }
    public DateTime? LastVaccineDate { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public Guid? UPPId { get; set; }
    public UPP? UPP { get; set; }
    public List<WeightRecord> WeightHistory { get; set; } = new();
    public List<Vaccine> Vaccines { get; set; } = new();
    public List<Treatment> Treatments { get; set; } = new();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
