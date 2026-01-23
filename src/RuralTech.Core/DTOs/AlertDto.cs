namespace RuralTech.Core.DTOs;

public class AlertDto
{
    public int AnimalId { get; set; }
    public string AnimalName { get; set; } = string.Empty;
    public string VaccineName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysLeft { get; set; }
    public bool IsUrgent { get; set; }
}
