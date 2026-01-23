namespace RuralTech.Core.DTOs;

public class CreateColaboradorDto
{
    public Guid UPPId { get; set; }
    public string NombreAlias { get; set; } = string.Empty;
    public string Pin { get; set; } = string.Empty; // PIN en texto plano (se encriptará)
    public string? TelefonoContacto { get; set; }
    public string Rol { get; set; } = string.Empty; // "ENCARGADO", "OPERARIO", "VETERINARIO"
}
