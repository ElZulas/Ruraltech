namespace RuralTech.Core.DTOs;

public class ConfirmationEmailDto
{
    public string Email { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string TempPassword { get; set; } = string.Empty;
}
