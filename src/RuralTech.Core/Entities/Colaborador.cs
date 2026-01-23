namespace RuralTech.Core.Entities;

public enum RolColaborador
{
    ENCARGADO,    // Total local
    OPERARIO,     // Solo lectura/registro básico
    VETERINARIO
}

public enum EstatusColaborador
{
    ACTIVO,
    SUSPENDIDO
}

public class Colaborador
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UPPId { get; set; } // id_upp
    public UPP UPP { get; set; } = null!;
    public string NombreAlias { get; set; } = string.Empty; // nombre_alias
    public string PinAccesoHash { get; set; } = string.Empty; // pin_acceso_hash
    public string? TelefonoContacto { get; set; } // telefono_contacto
    public RolColaborador Rol { get; set; } // rol
    public EstatusColaborador Estatus { get; set; } = EstatusColaborador.ACTIVO; // estatus
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
