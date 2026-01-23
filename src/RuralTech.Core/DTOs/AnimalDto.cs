namespace RuralTech.Core.DTOs;

public class AnimalDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Sex { get; set; } = string.Empty;
    public decimal CurrentWeight { get; set; }
    public DateTime? LastVaccineDate { get; set; }
    public List<WeightRecordDto> WeightHistory { get; set; } = new();
    public List<VaccineDto> Vaccines { get; set; } = new();
    public List<TreatmentDto> Treatments { get; set; } = new();
}

public class WeightRecordDto
{
    public int Id { get; set; }
    public decimal Weight { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}

public class VaccineDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateApplied { get; set; }
    public DateTime NextDueDate { get; set; }
    public string? Notes { get; set; }
}

public class TreatmentDto
{
    public int Id { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string TreatmentDescription { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}

public class CreateAnimalDto
{
    public string Name { get; set; } = string.Empty;
    public string Breed { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string Sex { get; set; } = string.Empty;
    public decimal CurrentWeight { get; set; }
}

public class AddWeightRecordDto
{
    public decimal Weight { get; set; }
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}

public class AddVaccineDto
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateApplied { get; set; }
    public DateTime NextDueDate { get; set; }
    public string? Notes { get; set; }
}

public class AddTreatmentDto
{
    public string Condition { get; set; } = string.Empty;
    public string TreatmentDescription { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string? Notes { get; set; }
}
