namespace RuralTech.Core.DTOs;

public class ColaboradorDto
{
    public Guid Id { get; set; }
    public Guid UPPId { get; set; }
    public string NombreUPP { get; set; } = string.Empty;
    public string NombreAlias { get; set; } = string.Empty;
    public string? TelefonoContacto { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string Estatus { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
